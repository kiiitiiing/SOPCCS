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
using SOPCOVIDChecker.Models.CovidKayaModels;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Models.SopViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "RHUUsers")]
    public class SopController : Controller
    {
        private readonly SOPCCContext _context;

        public SopController(SOPCCContext context)
        {
            _context = context;
        }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #region TESTING
        public ActionResult<CKPatient> CovidKayaTest(int? id)
        {
            var patient = _context.Patient
                .Include(x=>x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .SingleOrDefault(x=>x.Id == id);

            var ckpatient = new CKPatient
            {
                ResourceType = "patient",
                Id = patient.Id.ToString(),
                Name = new List<CKName> {
                    new CKName
                    {
                        Use = "official",
                        Family = patient.Lname,
                        Give = {patient.Fname, patient.Mname}
                    }
                },
                Telcom = new List<CKTelcom>
                {
                    new CKTelcom
                    {
                        System = "phone",
                        Value = patient.ContactNo
                    }
                },
                Gender = patient.Sex,
                BirthDate = patient.Dob.ToString("yyyy-MM-dd"),
            };
            return ckpatient;
        }
        #endregion
        #region SOP FORM LIST
        [HttpGet]
        public async Task<ActionResult<List<SopLess>>> SopFormJson(string q, string dr, string f)
        {
            if(!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.Parse(dr.Substring(0, dr.IndexOf(" ") + 1).Trim());
                EndDate = DateTime.Parse(dr.Substring(dr.LastIndexOf(" ")).Trim()).AddDays(1).AddSeconds(-1);
            }

            var sop = await _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.ResultForm)
                .Include(x => x.DiseaseReportingUnit)
                .Where(x=>x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                .Where(x => x.DiseaseReportingUnitId == UserFacility)
                //.Where(x => x.ResultForm.First().ApprovedBy == null)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new SopLess
                {
                    SampleId = x.SampleId,
                    PatientName = x.Patient.GetFullName(),
                    Age = x.Patient.Dob.ComputeAge(),
                    Sex = x.Patient.Sex,
                    DateOfBirth = x.Patient.Dob,
                    PCRResult = x.PcrResult,
                    DRU = string.IsNullOrEmpty(x.DiseaseReportingUnit.Abbr) ?
                        x.DiseaseReportingUnit.Name : x.DiseaseReportingUnit.Abbr,
                    Address = x.Patient.GetAddress(),
                    DateTimeCollection = x.DatetimeCollection,
                    RequestedBy = x.RequestedBy,
                    RequesterContact = x.RequesterContact,
                    SpecimenCollection = x.TypeSpecimen,
                    DateTimeReceipt = x.DatetimeSpecimenReceipt,
                    DateResult = x.DateResult,
                    DateOnset = x.DateOnsetSymptoms,
                    Swabber = x.Swabber
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
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            return View();
        }

        public IActionResult SopIndexPartial([FromBody]IEnumerable<SopLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region ADD SOP FORM
        public IActionResult AddSopModal(int patientId)
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSopModal(Sopform model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            model.DateResult = default;
            model.DatetimeSpecimenReceipt = default;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            model.DiseaseReportingUnitId = UserFacility;
            model.ResultForm.Add(NewResultForm());

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();

                return PartialView(model);
            }

            ViewBag.Errors = errors;

            return PartialView(model);
        }
        #endregion
        #region STATUS
        public async Task<ActionResult<List<ResultLess>>> SampleStatusJson(string q, string dr, string f)
        {

            if (!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.Parse(dr.Substring(0, dr.IndexOf(" ") + 1).Trim());
                EndDate = DateTime.Parse(dr.Substring(dr.LastIndexOf(" ")).Trim()).AddDays(1).AddSeconds(-1);
            }

            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x=>x.UpdatedAt >= StartDate && x.UpdatedAt <= EndDate)
                .Where(x=>x.SopForm.DiseaseReportingUnitId == UserFacility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    Lab = x.CreatedByNavigation.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.Result()
                })
                .ToListAsync();

            if(!string.IsNullOrEmpty(q))
            {
                sop = sop.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if(!string.IsNullOrEmpty(f))
            {
                sop = sop.Where(x => x.Status.Equals(f)).ToList();
            }

            

            return sop;
        }
        public IActionResult SampleStatus()
        {
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            return View();
        }
        public IActionResult SampleStatusPartial([FromBody]IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region PATIENTS
        [HttpGet]
        public async Task<ActionResult<List<ListPatientModel>>> PatientsJson(string q, string dr, string f)
        {
            var patients = await _context.Patient
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ListPatientModel
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    DateOfBirth = x.Dob.GetDate("MMM d, yyyy"),
                    Age = x.Dob.ComputeAge(),
                    Sex = x.Sex,
                    Address = x.GetAddress(),
                    BarangayId = x.Barangay,
                    MuncutyId = x.Muncity,
                    ProvinceId = x.Province
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(q))
            {
                patients = patients.Where(x => x.Name.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return patients;
        }
        public IActionResult Patients()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PatientsPartial([FromBody] IEnumerable<ListPatientModel> model)
        {
            return PartialView(model);
        }
        #endregion
        #region ADD PATIENT

        public IActionResult AddPatient()
        {
            ViewBag.Muncity = GetMuncities(UserProvince);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(Patient model)
        {
            model.Province = 2;
            model.CreatedAt = DateTime.Now;
            model.UpdatedAt = DateTime.Now;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (model.Muncity != 0)
            {
                ViewBag.Muncity = GetMuncities(UserProvince);
            }
            if (model.Barangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.Muncity, model.Province);
            }
            if (ModelState.IsValid)
            {
                if(_context.Patient.Any(x=>
                    x.Fname.ToUpper().Equals(model.Fname.ToUpper()) &&
                    (x.Mname??"").ToUpper().Equals((model.Mname??"").ToUpper()) &&
                    x.Lname.ToUpper().Equals(model.Lname.ToUpper()) &&
                    x.Sex == model.Sex &&
                    x.Dob == model.Dob))
                {
                    ViewBag.Duplicate = "Patient already exists";
                    return PartialView(model);
                }
                else
                {
                    await _context.AddAsync(model);
                    await _context.SaveChangesAsync();
                    return PartialView(model);

                }
            }
            ViewBag.Errors = errors;
            return PartialView(model);
        }
        #endregion
        #region EDIT PATIENT

        public async Task<IActionResult> EditPatient(int patientId)
        {
            var patient = await _context.Patient
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .FirstOrDefaultAsync(x => x.Id == patientId);
            if (patient.Muncity != 0)
            {
                ViewBag.Muncity = GetMuncities(patient.Province);
            }
            if (patient.Barangay != 0)
            {
                ViewBag.Barangay = GetBarangays(patient.Muncity, patient.Province);
            }
            return PartialView(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(Patient model)
        {
            model.UpdatedAt = DateTime.Now;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (_context.Patient.Where(x=>x.Id != model.Id).Any(x =>
                     x.Fname.ToUpper().Equals(model.Fname.ToUpper()) &&
                     (x.Mname ?? "").ToUpper().Equals((model.Mname ?? "").ToUpper()) &&
                     x.Lname.ToUpper().Equals(model.Lname.ToUpper()) &&
                     x.Sex == model.Sex &&
                     x.Dob == model.Dob))
                {
                    ViewBag.Duplicate = "Patient already exists";
                    return PartialView(model);
                }
                else
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    return PartialView(model);
                }
            }
            ViewBag.Errors = errors;
            if (model.Muncity != 0)
            {
                ViewBag.Muncity = GetMuncities(UserProvince);
            }
            if (model.Barangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.Muncity, model.Province);
            }
            return PartialView(model);
        }
        #endregion
        #region HELPERS

        public ResultForm NewResultForm()
        {
            var form = new ResultForm
            {
                PerformedBy = null,
                VerifiedBy = null,
                ApprovedBy = null,
                CreatedBy = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return form;
        }

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
