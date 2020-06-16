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

        #region
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region RESULT FORM
        public async Task<IActionResult> ResultForm(string sampleId)
        {
            var sop = await _context.Sopform
                .Include(x => x.Patient)
                .Include(x => x.DiseaseReportingUnit)
                .SingleOrDefaultAsync(x => x.SampleId == sampleId);

            return PartialView(new ResultForm
            {
                SopForm = sop,
                SopFormId = sop.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
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
