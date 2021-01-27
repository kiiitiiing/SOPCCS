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
using SOPCOVIDChecker.Models.AdminViewModel;
using SOPCOVIDChecker.Models.SopViewModel;
using SOPCOVIDChecker.Models.ViewModels;
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

        public async Task<ActionResult<ChartModel>> GetChartvalues()
        {
            var dateNow = DateTime.Now;
            var startDate = new DateTime(dateNow.Year, dateNow.Month, 1);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);
            var lastDay = endDate.Day;
            var days = new List<string>();
            var positive = new List<int>();
            var negative = new List<int>();
            var invalid = new List<int>();
            var processing = new List<int>();
            var sop = _context.Sopform
                .Where(x => x.UpdatedAt >= startDate && x.UpdatedAt <= endDate);
            for (int c = 1; c <= lastDay; c++)
            {
                days.Add(c.ToString());
                positive.Add( await sop.Where(x => x.UpdatedAt.Day == c && x.PcrResult.Contains("detected")).CountAsync());
                negative.Add(await sop.Where(x => x.UpdatedAt.Day == c && x.PcrResult.Contains("not")).CountAsync());
                invalid.Add(await sop.Where(x => x.UpdatedAt.Day == c && x.PcrResult.Contains("Invalid")).CountAsync());
                processing.Add(await sop.Where(x => x.UpdatedAt.Day == c && x.PcrResult.Contains("none")).CountAsync());
            }
            var chart = new ChartModel
            {
                Day = days,
                Positive = positive,
                Negative = negative,
                Invalid = invalid,
                Processing = processing
            };

            return chart;
        }
        #endregion

        #region ADD FACILITY
        // GET: ADD FACILITY    
        public IActionResult AddFacility()
        {
            var provinces = _context.Province;
            ViewBag.Province = new SelectList(provinces, "Id", "Description");
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
                var returnFacilities = await _context.Facility
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

                var returnModel = PaginatedList<FacilitiesModel>.CreateAsync(returnFacilities, ControllerContext.Action(), 1, 10);
                return Json(new JsonModel
                {
                    IsValid = ModelState.IsValid,
                    Html = Helper.RenderRazorViewToString(this, nameof(FacilitiesPartial), returnModel),
                    Toast = $"Facilit [{model.Name}] added successfully"
                });
            }
            else
            {
                if (model.Province == 0)
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

            return Json(new JsonModel
            {
                IsValid = false,
                Html = Helper.RenderRazorViewToString(this, nameof(UpdateFacility), model),
                Toast = ""
            });
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

            ViewBag.Province = new SelectList(province, "Id", "Description", facility.Province);
            ViewBag.Muncity = new SelectList(muncity, "Id", "Description", facility.Muncity);
            ViewBag.Barangay = new SelectList(barangay, "Id", "Description", facility.Barangay);
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
                    var returnFacilities = await _context.Facility
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

                    var returnModel = PaginatedList<FacilitiesModel>.CreateAsync(returnFacilities, ControllerContext.Action(), 1, 10);
                    return Json(new JsonModel
                    {
                        IsValid = ModelState.IsValid,
                        Html = Helper.RenderRazorViewToString(this, nameof(FacilitiesPartial), returnModel),
                        Toast = $"Facilit [{model.Name}] updated successfully"
                    });
                }
                else
                {
                    ModelState.AddModelError("Name", "Facility name already exists!");
                }
            }

            var province = _context.Province;
            var muncity = _context.Muncity.Where(x => x.Province.Equals(model.Province));
            var barangay = _context.Barangay.Where(x => x.Muncity.Equals(model.Barangay));

            ViewBag.Province = new SelectList(province, "Id", "Description");
            ViewBag.Muncity = new SelectList(muncity, "Id", "Description");
            ViewBag.Barangay = new SelectList(barangay, "Id", "Description");
            ViewBag.Levels = new SelectList(ListContainer.HospitalLevel, "Key", "Value");
            ViewBag.Types = new SelectList(ListContainer.HospitalType, "Key", "Value");

            return Json(new JsonModel
            {
                IsValid = false,
                Html = Helper.RenderRazorViewToString(this, nameof(UpdateFacility), model),
                Toast = ""
            });
        }
        #endregion

        #region FACILITES
        // FACILITIES
        public IActionResult Facilities()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FacilitiesPartial(string q, int? p)
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

            int s = 10;

            return PartialView(PaginatedList<FacilitiesModel>.CreateAsync(facilities, ControllerContext.Action(), p ?? 1, s));
        }
        #endregion

        #region RHU
        public IActionResult RhuUsers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RhuUsersPartial(string q, int? p)
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
                    Fname = x.Fname,
                    Mname = x.Mname,
                    Lname = x.Lname,
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                }).ToListAsync();


            if (!string.IsNullOrEmpty(q))
            {
                rhuUsers = rhuUsers.Where(x =>
                    x.Fullname.Contains(q) || x.Username.Contains(q)).ToList();
            }

            int s = 10;

            return PartialView(PaginatedList<UserLess>.CreateAsync(rhuUsers, ControllerContext.Action(), p ?? 1, s));
        }
        #endregion

        #region PESU
        public IActionResult PesuUsers()
        {
            return View();
        }

        public async Task<IActionResult> PesuUsersPartial(string q, int? p)
        {
            var users = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("PESU"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Fname = x.Fname,
                    Mname = x.Mname,
                    Lname = x.Lname,
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(q))
            {
                users = users.Where(x => x.Fullname.Contains(q) || x.Username.Contains(q)).ToList();
            }

            int s = 10;

            return PartialView(PaginatedList<UserLess>.CreateAsync(users, ControllerContext.Action(), p ?? 1, s));
        }
        #endregion

        #region RESU
        public IActionResult ResuUsers()
        {
            return View();
        }

        public async Task<IActionResult> ResuUsersPartial(string q, int? p)
        {
            var users = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("RESU"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Fname = x.Fname,
                    Mname = x.Mname,
                    Lname = x.Lname,
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                users = users.Where(x => x.Fullname.Contains(q) || x.Username.Contains(q)).ToList();
            }

            int s = 10;

            return PartialView(PaginatedList<UserLess>.CreateAsync(users, ControllerContext.Action(), p ?? 1, s));
        }
        #endregion

        #region LAB
        public IActionResult LabUsers()
        {
            return View();
        }

        public async Task<IActionResult> LabUsersPartial(string q, int? p)
        {
            var users = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x => x.UserLevel.Equals("LAB"))
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Fname = x.Fname,
                    Mname = x.Mname,
                    Lname = x.Lname,
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Address = x.GetAddress(),
                    Facility = x.Facility.Name,
                    Username = x.Username
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                users = users.Where(x => x.Fullname.Contains(q) || x.Username.Contains(q)).ToList();
            }

            int s = 10;

            return PartialView(PaginatedList<UserLess>.CreateAsync(users, ControllerContext.Action(), p ?? 1, s));
        }
        #endregion

        #region PATIENTS
        [HttpGet]
        public IActionResult Patients()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PatientsPartial(string q, int? page)
        {
            var patients = _context.Patient
                   .Include(x => x.CurrentBarangayNavigation)
                   .Include(x => x.CurrentMuncityNavigation)
                   .Include(x => x.CurrentProvinceNavigation)
                   .OrderByDescending(x => x.CreatedAt)
                   .Select(x => new ListPatientModel
                   {
                       Id = x.Id,
                       Name = x.Fname + " " + (x.Mname ?? "") + " " + x.Lname,
                       DateOfBirth = x.Dob.GetDate("MMM d, yyyy"),
                       Age = x.Dob.ComputeAge(),
                       Sex = x.Sex,
                       Address = x.GetAddress(),
                       BarangayId = x.CurrentBarangay,
                       MuncutyId = x.CurrentMuncity,
                       ProvinceId = x.CurrentProvince
                   });

            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                patients = patients.Where(x => x.Name.Contains(q));
            }
            int size = 10;

            return PartialView(PaginatedList<ListPatientModel>.CreateAsync(await patients.ToListAsync(), ControllerContext.Action(), page ?? 1, size));
        }
        #endregion

        #region EDIT PATIENT

        public async Task<IActionResult> EditPatient(int patientId)
        {
            var patient = await _context.Patient
                .Include(x => x.CurrentBarangayNavigation)
                .Include(x => x.CurrentMuncityNavigation)
                .Include(x => x.CurrentProvinceNavigation)
                .FirstOrDefaultAsync(x => x.Id == patientId);
            if (patient.CurrentMuncity != 0)
            {
                ViewBag.Muncity = GetMuncities(patient.CurrentProvince);
            }
            if (patient.CurrentBarangay != 0)
            {
                ViewBag.Barangay = GetBarangays(patient.CurrentMuncity, patient.CurrentProvince);
            }

            var patientModel = await GetPatientModel(patient);
            return PartialView(patientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(AddPatientModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (_context.Patient.Where(x => x.Id != model.Id).Any(x =>
                       x.Fname.ToUpper().Equals(model.Fname.ToUpper()) &&
                       (x.Mname ?? "").ToUpper().Equals((model.Mname ?? "").ToUpper()) &&
                       x.Lname.ToUpper().Equals(model.Lname.ToUpper()) &&
                       x.Sex == model.Sex &&
                       x.Dob == model.Dob))
                {
                    ViewBag.Duplicate = "Patient already exists";
                    return Json(new JsonModel
                    {
                        IsValid = false,
                        Html = Helper.RenderRazorViewToString(this, nameof(EditPatient), model),
                        Toast = $"[{model.Fname} {model.Lname}] already exists"
                    });
                }
                else
                {
                    var patient = await SetPatient(model);
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    return Json(new JsonModel
                    {
                        IsValid = true,
                        Html = Helper.RenderRazorViewToString(this, nameof(PatientsPartial), await GetPatients(1,10,"PatientsPartial")),
                        Toast = $"[{model.Fname} {model.Lname}] updated"
                    });
                }
            }
            ViewBag.Errors = errors;
            if (model.CurrentMuncity != 0)
            {
                ViewBag.Muncity = GetMuncities(UserProvince);
            }
            if (model.CurrentBarangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.CurrentMuncity, model.CurrentProvince);
            }
            return Json(new JsonModel
            {
                IsValid = false,
                Html = Helper.RenderRazorViewToString(this, nameof(EditPatient), model),
                Toast = $"Edit of [{model.Fname} {model.Lname}] failed"
            });
        }
        #endregion

        #region HELPERS
        public async Task<PaginatedList<ListPatientModel>> GetPatients(int? page, int size, string action)
        {
            var patients = await _context.Patient
                   .Include(x => x.CurrentBarangayNavigation)
                   .Include(x => x.CurrentMuncityNavigation)
                   .Include(x => x.CurrentProvinceNavigation)
                   .OrderByDescending(x => x.CreatedAt)
                   .Select(x => new ListPatientModel
                   {
                       Id = x.Id,
                       Name = x.Fname + " " + (x.Mname ?? "") + " " + x.Lname,
                       DateOfBirth = x.Dob.GetDate("MMM d, yyyy"),
                       Age = x.Dob.ComputeAge(),
                       Sex = x.Sex,
                       Address = x.GetAddress(),
                       BarangayId = x.CurrentBarangay,
                       MuncutyId = x.CurrentMuncity,
                       ProvinceId = x.CurrentProvince
                   }).ToListAsync();

            return PaginatedList<ListPatientModel>.CreateAsync(patients, action, page ?? 1, size);
        }

        public SelectList GetBarangays(int muncityId, int provinceId)
        {
            return new SelectList(_context.Barangay.Where(x => x.Muncity == muncityId && x.Province == x.Province), "Id", "Description");
        }

        public Task<Patient> SetPatient(AddPatientModel model)
        {
            var patient = new Patient
            {
                Fname = model.Fname,
                Mname = model.Mname,
                Lname = model.Lname,
                Sex = model.Sex,
                Dob = (DateTime)model.Dob,
                ContactNo = model.ContactNo,
                Email = model.Email,
                CurrentBarangay = model.CurrentBarangay,
                CurrentMuncity = model.CurrentMuncity,
                CurrentProvince = 2,
                CurrentPurok = model.CurrentPurok,
                CurrentSitio = model.CurrentSitio,
                CurrentAddress = model.CurrentAddress,
                PermanentBarangay = model.Disabled ? model.CurrentBarangay : model.PermanentBarangay,
                PermanentMuncity = model.Disabled ? model.CurrentMuncity : model.PermanentMuncity,
                PermanentProvince = 2,
                PermanentAddress = model.Disabled ? model.CurrentAddress : model.PermanentAddress,
                PermanentPurok = model.Disabled ? model.CurrentPurok : model.PermanentPurok,
                PermanentSitio = model.Disabled ? model.CurrentSitio : model.PermanentSitio,
                PhicMembershipType = model.PhicMembershipType,
                Pin = model.PIN,
                Employed = model.Employed,
                Pen = model.Employed ? model.PEN : "Unemployed",
                EmployerName = model.Employed ? model.EmployerName : "Unemployed",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return Task.FromResult(patient);
        }

        public Task<AddPatientModel> GetPatientModel(Patient patient)
        {
            var model = new AddPatientModel
            {
                Fname = patient.Fname,
                Mname = patient.Mname,
                Lname = patient.Lname,
                Sex = patient.Sex,
                Dob = (DateTime)patient.Dob,
                ContactNo = patient.ContactNo,
                Email = patient.Email,
                CurrentBarangay = patient.CurrentBarangay,
                CurrentMuncity = patient.CurrentMuncity,
                CurrentProvince = 2,
                CurrentPurok = patient.CurrentPurok,
                CurrentSitio = patient.CurrentSitio,
                CurrentAddress = patient.CurrentAddress,
                PermanentBarangay = patient.PermanentBarangay,
                PermanentMuncity = patient.PermanentMuncity,
                PermanentProvince = 2,
                PermanentAddress = patient.PermanentAddress,
                PermanentPurok = patient.PermanentPurok,
                PermanentSitio = patient.PermanentSitio,
                PhicMembershipType = patient.PhicMembershipType,
                PIN = patient.Pin,
                Employed = patient.Employed,
                PEN = patient.Pen,
                EmployerName = patient.EmployerName
            };

            return Task.FromResult(model);
        }

        public SelectList GetMuncities(int id)
        {
            return new SelectList(_context.Muncity.Where(x => x.Province == id), "Id", "Description");
        }
        private async Task<Facility> SaveFacilityAsync(FacilityModel model)
        {
            var facility = await _context.Facility.FindAsync(model.Id);
            facility.Name = model.Name;
            facility.Abbr = model.Abbrevation;
            facility.Province = model.Province;
            facility.Muncity = model.Muncity;
            facility.Barangay = model.Barangay;
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
        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity"));
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);
        #endregion
    }
}
