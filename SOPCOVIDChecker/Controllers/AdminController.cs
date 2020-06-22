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
using SOPCOVIDChecker.Models.AdminViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly SOPCCContext _context;

        public AdminController(SOPCCContext context)
        {
            _context = context;
        }

        #region HOME
        public IActionResult Index()
        {
            return View();
        }
        #endregion
        #region ADD FACILITY
        // GET: ADD FACILITY    
        public IActionResult AddFacility()
        {
            var provinces = _context.Province;
            ViewBag.Provinces = new SelectList(provinces, "Id", "Description");
            ViewBag.HospitalLevels = new SelectList(ListContainer.HospitalLevel, "Key", "Value");
            ViewBag.HospitalTypes = new SelectList(ListContainer.HospitalType, "Key", "Value");
            return PartialView();
        }

        // POST: ADD FACILITY
        [HttpPost]
        public async Task<IActionResult> AddFacility([Bind] FacilityModel model)
        {
            if (ModelState.IsValid)
            {
                var faciliy = await SetFacilityViewModelAsync(model);
                await _context.AddAsync(faciliy);
                await _context.SaveChangesAsync();
                return PartialView();
            }
            else
            {
                if (model.Province == null)
                    ModelState.AddModelError("Province", "Please select a province.");
                if (model.Muncity == null)
                    ModelState.AddModelError("Muncity", "Please select a municipality/city.");
                if (model.Barangay == null)
                    ModelState.AddModelError("Barangay", "Please select a barangay.");
                if (model.Level == null)
                    ModelState.AddModelError("Level", "Please select hospital level.");
                if (model.Type == null)
                    ModelState.AddModelError("Type", "Plase select hospital type.");
            }
            var provinces = _context.Province;
            ViewBag.Provinces = new SelectList(provinces, "Id", "Description", model.Province);
            if (model.Muncity != null)
            {
                var muncities = _context.Muncity.Where(x => x.Province == model.Province);
                ViewBag.Muncities = new SelectList(muncities, "Id", "Description", model.Muncity);
            }
            if (model.Barangay != null)
            {
                var barangays = _context.Barangay.Where(x => x.Province == model.Province && x.Muncity == model.Muncity);
                ViewBag.Barangays = new SelectList(barangays, "Id", "Description", model.Barangay);
            }
            ViewBag.HospitalLevels = new SelectList(ListContainer.HospitalLevel, "Key", "Value", model.Level);
            ViewBag.HospitalTypes = new SelectList(ListContainer.HospitalType, "Key", "Value", model.Type);
            return PartialView(model);
        }
        #endregion
        #region UPDATE FACILITY
        [HttpGet]
        public async Task<IActionResult> UpdateFacility(int? id)
        {
            var facilityModel = await _context.Facility.FindAsync(id);

            var facility = await SetFacilityModel(facilityModel);

            var province = _context.Province;
            var muncity = _context.Muncity.Where(x => x.Province.Equals(facility.Province));
            var barangay = _context.Barangay.Where(x => x.Muncity.Equals(facility.Muncity));

            ViewBag.Provinces = new SelectList(province, "Id", "Description", facility.Province);
            ViewBag.Muncities = new SelectList(muncity, "Id", "Description", facility.Muncity);
            ViewBag.Barangays = new SelectList(barangay, "Id", "Description", facility.Barangay);
            ViewBag.Levels = new SelectList(ListContainer.HospitalLevel, "Key", "Value", facility.Level);
            ViewBag.Types = new SelectList(ListContainer.HospitalType, "Key", "Value", facility.Type);

            return PartialView(facility);
        }

        // POST: UPDATE FACILITY
        [HttpPost]
        public async Task<IActionResult> UpdateFacility([Bind] FacilityModel model)
        {
            if (string.IsNullOrEmpty(model.Chief))
                model.Chief = "";
            if (ModelState.IsValid)
            {
                var facilities = _context.Facility.Where(x => x.Id != model.Id);
                if (!facilities.Any(x => x.Name.Equals(model.Name)))
                {
                    var saveFacility = await SaveFacilityAsync(model);
                    _context.Update(saveFacility);
                    await _context.SaveChangesAsync();
                    return PartialView("~/Views/Admin/UpdateFacility.cshtml", model);
                }
                else
                {
                    ModelState.AddModelError("Name", "Facility name already exists!");
                }
            }

            var province = _context.Province;
            var muncity = _context.Muncity.Where(x => x.Province.Equals(model.Province));
            var barangay = _context.Barangay.Where(x => x.Muncity.Equals(model.Barangay));

            ViewBag.Provinces = new SelectList(province, "Id", "Description", model.Province);
            ViewBag.Muncities = new SelectList(muncity, "Id", "Description", model.Muncity);
            ViewBag.Barangays = new SelectList(barangay, "Id", "Description", model.Barangay);
            ViewBag.Levels = new SelectList(ListContainer.HospitalLevel, "Key", "Value", model.Level);
            ViewBag.Types = new SelectList(ListContainer.HospitalType, "Key", "Value", model.Type);

            return PartialView(model);
        }
        #endregion
        #region FACILITES
        public async Task<ActionResult<List<FacilitiesModel>>> FacilitiesJson(string q)
        {
            var facilities = await _context.Facility
                            .Select(x => new FacilitiesModel
                            {
                                Id = x.Id,
                                Facility = x.Name,
                                Address = x.GetAddress(),
                                Contact = x.ContactNo,
                                Email = x.Email,
                                Chief = x.ChiefHospital,
                                Level = x.Level,
                                Type = x.HospitalType
                            })
                            .ToListAsync();


            if (!string.IsNullOrEmpty(q))
            {
                facilities = facilities.Where(x => x.Facility.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return facilities;
        }
        // FACILITIES
        public IActionResult Facilities()
        {
            return View();
        }

        public IActionResult FacilitiesPartial([FromBody]IEnumerable<FacilitiesModel> model)
        {
            return PartialView(model);
        }
        #endregion
        #region RHU
        public async Task<ActionResult<List<UserLess>>> RhuJson(string q)
        {
            var rhuUsers = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("RHU"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            return rhuUsers;
        }
        public IActionResult RhuUsers()
        {
            return View();
        }

        public IActionResult RhuUsersPartial([FromBody]IEnumerable<UserLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region PESU
        public async Task<ActionResult<List<UserLess>>> PesuJson(string q)
        {
            var rhuUsers = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("PESU"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            return rhuUsers;
        }
        public IActionResult PesuUsers()
        {
            return View();
        }

        public IActionResult PesuUsersPartial([FromBody] IEnumerable<UserLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region RESU
        public async Task<ActionResult<List<UserLess>>> ResuJson(string q)
        {
            var rhuUsers = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("RESU"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            return rhuUsers;
        }
        public IActionResult ResuUsers()
        {
            return View();
        }

        public IActionResult ResuUsersPartial([FromBody] IEnumerable<UserLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region LAB
        public async Task<ActionResult<List<UserLess>>> LabJson(string q)
        {
            var rhuUsers = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x=>x.UserLevel.Equals("LAB"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            return rhuUsers;
        }
        public IActionResult LabUsers()
        {
            return View();
        }

        public IActionResult LabUsersPartial([FromBody] IEnumerable<UserLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region HELPERS
        private async Task<Facility> SaveFacilityAsync(FacilityModel model)
        {
            var facility = await _context.Facility.FindAsync(model.Id);
            facility.Name = model.Name;
            facility.Abbr = model.Abbrevation;
            facility.Province = (int)model.Province;
            facility.Muncity = (int)model.Muncity;
            facility.Barangay = (int)model.Barangay;
            facility.Address = model.Address;
            facility.ContactNo = model.Contact;
            facility.Email = model.Email;
            facility.ChiefHospital = model.Chief;
            facility.Level = model.Level;
            facility.HospitalType = model.Type;

            return facility;
        }

        private Task<FacilityModel> SetFacilityModel(Facility facility)
        {
            var facilityModel = new FacilityModel
            {
                Id = facility.Id,
                Name = facility.Name,
                Abbrevation = facility.Abbr,
                Province = facility.Province,
                Muncity = facility.Muncity,
                Barangay = facility.Barangay,
                Address = facility.Address,
                Contact = facility.ContactNo,
                Email = facility.Email,
                Chief = facility.ChiefHospital,
                Level = facility.Level,
                Type = facility.HospitalType
            };

            return Task.FromResult(facilityModel);
        }

        private Task<Facility> SetFacilityViewModelAsync(FacilityModel model)
        {
            var facility = new Facility
            {
                Name = model.Name,
                Abbr = model.Abbrevation,
                Address = model.Address,
                Barangay = model.Barangay,
                Muncity = model.Muncity,
                Province = (int)model.Province,
                ContactNo = model.Contact,
                Email = model.Email,
                Status = 1,
                Picture = "",
                ChiefHospital = model.Chief,
                Level = model.Level.ToString(),
                HospitalType = model.Type
            };

            return Task.FromResult(facility);
        }
        #endregion
    }
}
