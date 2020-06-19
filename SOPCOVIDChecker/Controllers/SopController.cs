using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Models.SopViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "RHUUsers")]
    public class SopController : Controller
    {
        private readonly SOPCCContext _context;

        public SopController(SOPCCContext context)
        {
            _context = context;
        }

        #region SOP FORM LIST
        [HttpGet]
        public async Task<ActionResult<List<SopLess>>> SopFormJson(string q)
        {
            var sop = await _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.ResultForm)
                .Include(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Where(x => x.DiseaseReportingUnit.FacilityId == UserFacility)
                .Where(x => x.ResultForm.First().CreatedBy == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new SopLess
                {
                    SampleId = x.SampleId,
                    PatientName = x.Patient.GetFullName(),
                    Age = x.Patient.Dob.ComputeAge(),
                    Sex = x.Patient.Sex,
                    DateOfBirth = x.Patient.Dob,
                    PCRResult = x.PcrResult,
                    DRU = string.IsNullOrEmpty(x.DiseaseReportingUnit.Facility.Abbr) ?
                        x.DiseaseReportingUnit.Facility.Name : x.DiseaseReportingUnit.Facility.Abbr,
                    Address = x.Patient.GetAddress(),
                    DateTimeCollection = x.DatetimeCollection,
                    RequestedBy = x.RequestedBy,
                    RequesterContact = x.RequesterContact,
                    SpecimenCollection = x.TypeSpecimen,
                    DateTimeReceipt = x.DatetimeSpecimenReceipt,
                    DateResult = x.DateResult
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                sop = sop.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return sop;
        }

        public IActionResult SopIndex()
        {
            return View();
        }

        public IActionResult SopIndexPartial([FromBody]IEnumerable<SopLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region ADD SOP FORM
        public IActionResult AddSopModal()
        {
            ViewBag.Province = GetProvinces();
            ViewBag.Muncity = GetMuncities(UserProvince);
            
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSopModal(Sopform model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            ViewBag.Muncity = GetMuncities(UserProvince);
            ViewBag.Province = GetProvinces();
            ViewBag.Barangay = GetBarangays(model.Patient.Muncity, model.Patient.Province);
            model.Patient.CreatedAt = DateTime.Now;
            model.Patient.UpdatedAt = DateTime.Now;
            model.DateResult = default;
            model.DatetimeSpecimenReceipt = default;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.DiseaseReportingUnitId = UserId;
            model.ResultForm.Add(NewResultForm());

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                return PartialView(model);
            }

            ViewBag.Errors = errors;

            return PartialView(model);
        }
        #endregion
        #region STATUS
        public async Task<ActionResult<List<ResultLess>>> SampleStatusJson(string q)
        {
            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x=>x.SopForm.DiseaseReportingUnit.FacilityId == UserFacility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    Lab = x.CreatedByNavigation.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.Result()
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                sop = sop.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return sop;
        }
        public IActionResult SampleStatus()
        {
            return View();
        }
        public IActionResult SampleStatusPartial([FromBody]IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region HELPERS

        public ResultForm NewResultForm()
        {
            var form = new ResultForm
            {
                LabTestPerformed = "",
                TestResult = "",
                FinalResult = "",
                Interpretation = "",
                PerformedBy = null,
                VerifiedBy = null,
                ApprovedBy = null,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return form;
        }

        public SelectList GetMuncities(int id)
        {
            return new SelectList(_context.Muncity.Where(x => x.Province == id), "Id", "Description");
        }


        public SelectList GetProvinces()
        {
            return new SelectList(_context.Province, "Id", "Description", UserProvince);
        }

        public SelectList GetBarangays(int muncityId, int provinceId)
        {
            return new SelectList(_context.Barangay.Where(x => x.Muncity == muncityId && x.Province == x.Province), "Id", "Description");
        }


        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity"));
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);
        #endregion
    }
}
