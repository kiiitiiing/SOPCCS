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
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "RESUUsers")]
    public class ResuController : Controller
    {
        private readonly SOPCCContext _context;

        public ResuController(SOPCCContext context)
        {
            _context = context;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region DASHBOARD

        public async Task<ActionResult<List<ResultLess>>> ResuFormJson(string q)
        {
            var form = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x=>x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.CreatedBy == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x=> new ResultLess
                {
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    DRU  = x.SopForm.DiseaseReportingUnit.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    SampleId = x.SopForm.SampleId,
                    SampleTaken = x.SopForm.DatetimeCollection
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                form = form.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase) || x.SampleId.Equals(q)).ToList();
            }

            return form;
        }

        public IActionResult ResuIndex()
        {
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            return View();
        }
        public IActionResult ResuIndexPartial([FromBody]IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion

        #region SEND TO LAB
        public IActionResult SendToLabModal(int formId)
        {
            ViewBag.Labs = GetLabs();
            return PartialView(new SendToLab { ResultFormId = formId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToLabModal(SendToLab model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            ViewBag.Labs = GetLabs();
            if (ModelState.IsValid)
            {
                var resultForm = await _context.ResultForm.SingleOrDefaultAsync(x=>x.Id == model.ResultFormId);
                resultForm.CreatedBy = model.LabAccId;
                _context.Update(resultForm);
                await _context.SaveChangesAsync();
                return PartialView(model);
            }
            ViewBag.Errors = errors;
            return PartialView(model);
        }
        #endregion


        #region RESULT
        public async Task<ActionResult<List<ResultLess>>> ResuStatusJson(string q, string dr, string f)
        {
            if (!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.Parse(dr.Substring(0, dr.IndexOf(" ") + 1).Trim());
                EndDate = DateTime.Parse(dr.Substring(dr.LastIndexOf(" ")).Trim()).AddDays(1).AddSeconds(-1);
            }

            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x=>x.UpdatedAt >= StartDate && x.UpdatedAt <= EndDate)
                .Where(x => x.SopForm.DiseaseReportingUnit.Facility.Province == UserProvince)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    Lab = x.CreatedByNavigation.Facility.Abbr,
                    DRU = x.SopForm.DiseaseReportingUnit.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.Result()
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(q))
            {
                sop = sop.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if(!string.IsNullOrEmpty(f))
            {
                sop = sop.Where(x => x.Status.Equals(f)).ToList();
            }

            return sop;
        }
        public IActionResult ResuStatus()
        {
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            ViewBag.Province = _context.Province.Find(UserProvince).Description;
            return View();
        }
        public IActionResult ResuStatusPartial([FromBody] IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion

        #region HELPERS
        public partial class LabSelect
        {
            public int LabId { get; set; }
            public string LabName { get; set; }
        }

        public SelectList GetLabs()
        {
            var labs = _context.Sopusers
                .Include(x => x.Facility)
                .Where(x => x.UserLevel == "LAB")
                .Select(x => new LabSelect
                {
                    LabId = x.Id,
                    LabName = x.Facility.Name
                });
            return new SelectList(labs, "LabId", "LabName");
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
