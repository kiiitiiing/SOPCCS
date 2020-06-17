using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "LABUsers")]
    public class ResultController : Controller
    {
        private readonly SOPCCContext _context;

        public ResultController(SOPCCContext context)
        {
            _context = context;
        }

        #region DASHBOARD
        public async Task<ActionResult<List<ResultLess>>> LabJson(string q)
        {
            var form = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ResultLess
                {
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    DRU = x.SopForm.DiseaseReportingUnit.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    SampleId = x.SopForm.SampleId,
                    SampleTaken = x.SopForm.DatetimeCollection
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(q))
            {
                form = form.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase) || x.SampleId.Equals(q)).ToList();
            }

            return form;
        }
        public IActionResult LabIndex()
        {
            return View();
        }
        public IActionResult LabIndexPartial([FromBody]IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion

        #region RESULT FORM
        public async Task<IActionResult> ResultForm(int resultId)
        {
            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .SingleOrDefaultAsync(x => x.Id == resultId);

            return PartialView(sop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResultForm(ResultForm model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);

            if(ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return PartialView(model);
            }

            ViewBag.Errors = errors;
            return PartialView(model);
        }
        #endregion

        #region HELPERS
        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity"));
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);
        #endregion
    }
}
