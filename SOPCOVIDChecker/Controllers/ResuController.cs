using System;
using System.Collections.Generic;
using System.Linq;
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
                var resultForm = await _context.ResultForm.FindAsync(model.ResultFormId);
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
        #endregion
    }
}
