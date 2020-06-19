using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "PESUUsers")]
    public class PesuController : Controller
    {
        private readonly SOPCCContext _context;

        public PesuController(SOPCCContext context)
        {
            _context = context;
        }
        #region DASHBOARD
        public async Task<ActionResult<List<ResultLess>>> PesuStatusJson(string q)
        {
            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.SopForm.DiseaseReportingUnit.Facility.Province == UserProvince)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    Lab = x.CreatedByNavigation.Facility.Name,
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

            return sop;
        }
        public IActionResult PesuStatus()
        {
            ViewBag.Province = _context.Province.Find(UserProvince).Description;
            return View();
        }
        public IActionResult PesuStatusPartial([FromBody] IEnumerable<ResultLess> model)
        {
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
