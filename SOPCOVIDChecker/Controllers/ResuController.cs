using System;
using System.Collections.Generic;
using System.Globalization;
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
        public IActionResult ResuIndex()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResuIndexPartial(string q, int? page)
        {
            var form = _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.ApprovedBy == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ResultLess
                {
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.Fname + " " + (x.SopForm.Patient.Mname ?? "") + " " + x.SopForm.Patient.Lname,
                    DRU = x.SopForm.DiseaseReportingUnit.Name,
                    PCRResult = x.SopForm.PcrResult,
                    SampleId = x.SopForm.SampleId,
                    SampleTaken = x.SopForm.DatetimeCollection
                });

            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                form = form.Where(x => x.PatientName.Contains(q) || x.SampleId.Equals(q));
            }

            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            int size = 10;
            return PartialView(PaginatedList<ResultLess>.CreateAsync(await form.ToListAsync(), action, page ?? 1, size));
        }
        #endregion
        #region RESULT
        public IActionResult ResuStatus()
        {
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value");
            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResuStatusPartial(string q, string dr, string f, int? page)
        {
            ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value");
            if (!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.ParseExact(dr.Substring(0, dr.IndexOf(" ") + 1).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                EndDate = DateTime.ParseExact(dr.Substring(dr.LastIndexOf(" ")).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            }
            else
            {
                var date = DateTime.Now;
                StartDate = new DateTime(date.Year, date.Month, 1);
                EndDate = DateTime.Now.Date;
            }

            var sop = _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.UpdatedAt >= StartDate && x.UpdatedAt <= EndDate)
                .Where(x => x.SopForm.DiseaseReportingUnit.Province == UserProvince)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.Fname + " " + (x.SopForm.Patient.Mname ?? "") + " " + x.SopForm.Patient.Lname,
                    Lab = x.CreatedByNavigation.Facility.Abbr,
                    DRU = x.SopForm.DiseaseReportingUnit.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.ApprovedBy == null ? "PROCESSING" : "RELEASED"
                });

            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                sop = sop.Where(x => x.PatientName.Contains(q));
            }

            if (!string.IsNullOrEmpty(f))
            {
                ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value", f);
                sop = sop.Where(x => x.Status.Equals(f));
            }


            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            int size = 10;
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView(PaginatedList<ResultLess>.CreateAsync(await sop.ToListAsync(), action, page ?? 1, size));
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
