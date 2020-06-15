using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.SopViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    public class SopController : Controller
    {
        private readonly SOPCCContext _context;

        public SopController(SOPCCContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SopLess>>> SopFormJson(string q)
        {
            var sop = await _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.DiseaseReportingUnit)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x=> new SopLess 
                {
                    SampleId = x.SampleId,
                    PatientName = x.Patient.GetFullName(),
                    Age = x.Patient.Dob.ComputeAge(),
                    Sex = x.Patient.Sex,
                    DateOfBirth = x.Patient.Dob,
                    PCRResult = x.PcrResult,
                    DRU = x.DiseaseReportingUnit.Abbrevation,
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
            if(model.Patient.Barangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.Patient.Muncity, model.Patient.Province);
            }
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
