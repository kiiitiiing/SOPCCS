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
        public IActionResult Status()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> StatusPartial(string q, int? page)
        {
            var sop = _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.SopForm.DiseaseReportingUnit.Province == UserProvince)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.Fname+ " "+ (x.SopForm.Patient.Mname ?? "")+ " " + x.SopForm.Patient.Lname,
                    Lab = x.CreatedByNavigation.Facility.Name,
                    DRU = x.SopForm.DiseaseReportingUnit.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.ApprovedBy == null? "PROCESSING" : "RELEASED"
                });
            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                sop = sop.Where(x => x.PatientName.Contains(q));
            }

            int size = 10;
            return PartialView(PaginatedList<ResultLess>.CreateAsync(await sop.ToListAsync(), ControllerContext.Action(), page ?? 1, size));
        }
        #endregion

        #region HELPERS
        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserProvinceName => int.Parse(User.FindFirstValue("ProvinceName"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity")); 
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);
        #endregion
    }
}
