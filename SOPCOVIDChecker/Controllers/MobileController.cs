using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.MobileModels;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Route("api/[controller]")]
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
                    DiseaseReportingUnitId = getUser.Id,
                };
            }

            return null;
        }

        [Route("mobileapi/syncforms/{date}")]
        [HttpGet]
        public Task<List<Sopform>> GetSopForms(string date)
        {
            return _context.Sopform.Join
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
                 .Where(x => x.CreatedAt > DateTime.Parse(date))
                 .ToListAsync();
        }

        [Route("mobileapi/submitform")]
        [HttpPost]
        public async Task<bool> SubmitFormAsync(Sopform form)
        {
            form.Patient.CreatedAt = DateTime.Now;
            form.Patient.UpdatedAt = DateTime.Now;
            form.CreatedAt = DateTime.Now;
            form.UpdatedAt = DateTime.Now;
            _context.Add(form);
            return await _context.SaveChangesAsync() != 1;
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
