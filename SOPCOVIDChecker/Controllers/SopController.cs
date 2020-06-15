using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;

namespace SOPCOVIDChecker.Controllers
{
    public class SopController : Controller
    {
        private readonly SOPCCContext _context;

        public SopController(SOPCCContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

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
            model.Patient.CreatedAt = DateTime.Now;
            model.Patient.UpdatedAt = DateTime.Now;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.DiseaseReportingUnitId = UserFacility;

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                return PartialView(model);
            }

            ViewBag.Errors = errors;

            return PartialView(model);
        }

        #region HELPERS

        public SelectList GetMuncities(int id)
        {
            return new SelectList(_context.Muncity.Where(x => x.ProvinceId == id), "Id", "Description");
        }


        public SelectList GetProvinces()
        {
            return new SelectList(_context.Province, "Id", "Description", UserProvince);
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
