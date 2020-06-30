using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.MobileModels;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [ApiController]
    public class MobileController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;

        private readonly SOPCCContext _context;

        public MobileController(IUserService userService, IConfiguration configuration, SOPCCContext context)
        {
            _userService = userService;
            _configuration = configuration;
            _context = context;
        }

        [Route("mobileapi/login")]
        [HttpPost]
        public async Task<MainUserModel> Login(LoginModel usercredentials)
        {
            //LoginModel usercredentials
            var (isValid, user) = await _userService.ValidateUserCredentialsAsync(usercredentials.Username, usercredentials.Password);
            if (isValid)
            {
                var getUser = await _context.Sopusers.FirstOrDefaultAsync(x => x.Username == user.Username);
                var getFacility = await _context.Facility.FirstOrDefaultAsync(x => x.Id == getUser.FacilityId);

                return new MainUserModel
                {
                    Firstname = getUser.Fname,
                    Middlename = getUser.Mname,
                    Lastname = getUser.Lname,
                    UserLevel = getUser.UserLevel,
                    Facility = getFacility.Name,
                    FacilityId = getUser.FacilityId,
                    DiseaseReportingUnitId = getUser.Id,
                };
            }

            return null;
        }

        [Route("mobileapi/submitform")]
        [HttpPost]
        public async Task<(bool,int)> SubmitFormAsync(Sopform form)
        {
            if (ModelState.IsValid)
            {
                form.ResultForm.Add(Helpers.NewResultForm());
                _context.Sopform.Add(form);
                var result = await _context.SaveChangesAsync() != 1;
                var formId = await _context.Sopform.SingleOrDefaultAsync(x => x.CreatedAt == form.CreatedAt);
                return (result, formId.Id);
            }
            return (false,0);
        }

        [Route("mobileapi/syncpatients/")]
        [HttpGet]
        public async Task<List<Patient>> GetAllPatientsAsync(int count)
        {
            if ((await _context.Patient.ToListAsync()).Count() > count)
                return await _context.Patient.ToListAsync();
            else return new List<Patient>();
        }

        [Route("mobileapi/syncforms/")]
        [HttpGet]
        public Task<List<Sopform>> GetSopForms(int id, int ctr)
        {
            if(_context.Sopform.Where(x=>x.DiseaseReportingUnitId == id).Count() > ctr)
            {
                return _context.Sopform
                    .Include(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                    .Include(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                    .Include(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                    .Where(x => x.DiseaseReportingUnitId == id)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();
                /*return _context.Sopform.Join
                     (
                     _context.Patient,
                     sopform => sopform.PatientId,
                     patient => patient.Id, (sopform, patient) => new { sopform, patient })
                     .Select(sopform => new Sopform
                     {
                         Id = sopform.sopform.Id,
                         SampleId = sopform.sopform.SampleId,
                         PcrResult = sopform.sopform.PcrResult,
                         DiseaseReportingUnitId = sopform.sopform.DiseaseReportingUnitId,
                         DatetimeCollection = sopform.sopform.DatetimeCollection,
                         RequestedBy = sopform.sopform.RequestedBy,
                         RequesterContact = sopform.sopform.RequesterContact,
                         TypeSpecimen = sopform.sopform.TypeSpecimen,
                         DatetimeSpecimenReceipt = sopform.sopform.DatetimeSpecimenReceipt,
                         DateResult = sopform.sopform.DateResult,
                         PatientId = sopform.sopform.PatientId,
                         CreatedAt = sopform.sopform.CreatedAt,
                         UpdatedAt = sopform.sopform.UpdatedAt,
                         Patient = sopform.patient
                     })
                     .Where(x => x.DiseaseReportingUnit.FacilityId == id)
                     .ToListAsync();*/
            }
            else
            {
                return Task.FromResult(new List<Sopform>());
            }
        }

        [Route("mobileapi/patientform")]
        [HttpPost]
        public async Task<(bool,int)> SubmitPatientAsync(Patient form)
        {
            if(_context.Patient.Any(x=>
                    x.Fname.ToUpper().Equals(form.Fname.ToUpper()) &&
                    (x.Mname??"").ToUpper().Equals((form.Mname??"").ToUpper()) &&
                    x.Lname.ToUpper().Equals(form.Lname.ToUpper()) &&
                    x.Sex == form.Sex &&
                    x.Dob == form.Dob))
            {
                var patient = _context.Patient.SingleOrDefault(x =>
                    x.Fname.ToUpper().Equals(form.Fname.ToUpper()) &&
                    (x.Mname ?? "").ToUpper().Equals((form.Mname ?? "").ToUpper()) &&
                    x.Lname.ToUpper().Equals(form.Lname.ToUpper()) &&
                    x.Sex == form.Sex &&
                    x.Dob == form.Dob);
            }
            form.CreatedAt = DateTime.Now;
            form.UpdatedAt = DateTime.Now;
            _context.Add(form);
            var result = await _context.SaveChangesAsync() == 1;
            var savedForm = await _context.Patient.SingleOrDefaultAsync(x => x.CreatedAt.Equals(form.CreatedAt));
            return (result,savedForm.Id);
        }

        [Route("mobileapi/updateform")]
        [HttpPost]
        public async Task<bool> UpdateFormAsync(Sopform form)
        {
            _context.Update(form);
            return await _context.SaveChangesAsync() != 1;
        }

        [Route("mobileapi/barangays")]
        [HttpGet]
        public async Task<List<Barangay>> GetAllBarangays()
        {
            return await _context.Barangay.ToListAsync();
        }

        [Route("mobileapi/muncities")]
        [HttpGet]
        public async Task<List<Muncity>> GetAllMuncitiesAsync()
        {
            return await _context.Muncity.ToListAsync();
        }

        [Route("mobileapi/provinces")]
        [HttpGet]
        public async Task<List<Province>> GetAllProvincesAsync()
        {
            return await _context.Province.ToListAsync();
        }
    }
}
