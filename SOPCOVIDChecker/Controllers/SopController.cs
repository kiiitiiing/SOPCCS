using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
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
        #region DOWNLOAD
        public async Task<IActionResult> DownloadApp()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "assets","dist","apk", "com.companyname.sopcc.apk");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "APPLICATION/octet-stream", Path.GetFileName(path));
        }
        #endregion
        #region TESTING
        /*public ActionResult<Sopform> TestSop()
        {
            
        }*/
        public ActionResult<CKPatient> CovidKayaTest(int? id)
        {
            var patient = _context.Patient
                .Include(x=>x.CurrentBarangayNavigation)
                .Include(x => x.CurrentMuncityNavigation)
                .Include(x => x.CurrentProvinceNavigation)
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
                StartDate = DateTime.ParseExact(dr.Substring(0, dr.IndexOf(" ") + 1).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                EndDate = DateTime.ParseExact(dr.Substring(dr.LastIndexOf(" ")).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            }

            var sop = await _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentBarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentMuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentProvinceNavigation)
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
                    CAddress = x.Patient.GetAddress(),
                    PAddress = x.Patient.GetPermanentAddress(),
                    DateTimeCollection = x.DatetimeCollection,
                    RequestedBy = x.RequestedBy,
                    RequesterContact = x.RequesterContact,
                    SpecimenCollection = x.TypeSpecimen,
                    DateTimeReceipt = x.DatetimeSpecimenReceipt,
                    DateResult = x.DateResult,
                    DateOnset = (DateTime)x.DateOnsetSymptoms,
                    Swabber = x.Swabber,
                    ContactNo = x.Patient.ContactNo
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
            var model = new AddSopModel
            {
                PatientId = patientId,
                Disabled = false,
                DateOnsetSymptoms = null
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSopModal(AddSopModel model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);

            if(model.Disabled)
            {
                ModelState.Remove("DateOnsetSymptoms");
            }

            if (ModelState.IsValid)
            {
                var form = await SetSopform(model);
                _context.Add(form);
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
                StartDate = DateTime.ParseExact(dr.Substring(0, dr.IndexOf(" ") + 1).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                EndDate = DateTime.ParseExact(dr.Substring(dr.LastIndexOf(" ")).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            }

            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
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
                .Include(x => x.CurrentBarangayNavigation)
                .Include(x => x.CurrentMuncityNavigation)
                .Include(x => x.CurrentProvinceNavigation)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ListPatientModel
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    DateOfBirth = x.Dob.GetDate("MMM d, yyyy"),
                    Age = x.Dob.ComputeAge(),
                    Sex = x.Sex,
                    Address = x.GetAddress(),
                    BarangayId = x.CurrentBarangay,
                    MuncutyId = x.CurrentMuncity,
                    ProvinceId = x.CurrentProvince
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
        public async Task<IActionResult> AddPatient(AddPatientModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (model.CurrentMuncity != 0)
            {
                ViewBag.Muncity = GetMuncities(UserProvince);
            }
            if (model.CurrentBarangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.CurrentMuncity, model.CurrentProvince);
            }
            if(model.Disabled)
            {
                ModelState.Remove("PermanentBaragnay");
                ModelState.Remove("PermanentMuncity");
                ModelState.Remove("PermanentProvince");
                ModelState.Remove("PermanentAddress");
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
                    var patient = await SetPatient(model);
                    await _context.AddAsync(patient);
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
            if (model.CurrentMuncity != 0)
            {
                ViewBag.Muncity = GetMuncities(UserProvince);
            }
            if (model.CurrentBarangay != 0)
            {
                ViewBag.Barangay = GetBarangays(model.CurrentMuncity, model.CurrentProvince);
            }
            return PartialView(model);
        }
        #endregion
        #region EXCEL ALL
        public async Task<IActionResult> ExportAll()
        {
            var sop = await _context.Sopform
                   .Include(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                   .Include(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                   .Include(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                   .Include(x => x.Patient).ThenInclude(x => x.PermanentBarangayNavigation)
                   .Include(x => x.Patient).ThenInclude(x => x.PermanentMuncityNavigation)
                   .Include(x => x.Patient).ThenInclude(x => x.PermanentProvinceNavigation)
                   .Include(x => x.ResultForm)
                   .Include(x => x.DiseaseReportingUnit)
                   .Where(x => x.DiseaseReportingUnitId == UserFacility)
                   .ToListAsync();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "allpatients.xlsx";
            try
            {
                using(var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Authors");
                    worksheet.Cell(1, 1).Value = "SAMPLE ID";
                    worksheet.Cell(1, 2).Value = "PATIENT NAME";
                    worksheet.Cell(1, 3).Value = "AGE";
                    worksheet.Cell(1, 4).Value = "SEX";
                    worksheet.Cell(1, 5).Value = "DATE OF BIRTH";
                    worksheet.Cell(1, 6).Value = "CONTACT NUMBER";
                    worksheet.Cell(1, 7).Value = "CURRENT ADDRESS";
                    worksheet.Cell(1, 8).Value = "PERMANENT ADDRESS";
                    worksheet.Cell(1, 9).Value = "DISEASE REPORTING UNIT";
                    worksheet.Cell(1, 10).Value = "PCR RESULT";
                    worksheet.Cell(1, 11).Value = "DATE & TIME OF COLLECTION";
                    worksheet.Cell(1, 12).Value = "REQUESTED BY";
                    worksheet.Cell(1, 13).Value = "CONTACT NUMBER";
                    worksheet.Cell(1, 14).Value = "TYPE OF SPECIMEN & COLLECTION MEDIUM";
                    worksheet.Cell(1, 15).Value = "DATE & TIME OF SPECIMEN RECEIPT";
                    worksheet.Cell(1, 16).Value = "DATE OF RESULT RELEASE";
                    for (int index = 1; index <= sop.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value =  sop[index - 1].SampleId;
                        worksheet.Cell(index + 1, 2).Value = sop[index - 1].Patient.GetFullName();
                        worksheet.Cell(index + 1, 3).Value = sop[index - 1].Patient.Dob.ComputeAge();
                        worksheet.Cell(index + 1, 4).Value = sop[index - 1].Patient.Sex;
                        worksheet.Cell(index + 1, 5).Value = sop[index - 1].Patient.Dob.ToString("dd-MMM-yyyy");
                        worksheet.Cell(index + 1, 6).SetDataType(XLDataType.Text);
                        worksheet.Cell(index + 1, 6).Value = sop[index - 1].Patient.ContactNo;
                        worksheet.Cell(index + 1, 7).Value = sop[index - 1].Patient.GetAddress();
                        worksheet.Cell(index + 1, 8).Value = sop[index - 1].Patient.GetPermanentAddress();
                        worksheet.Cell(index + 1, 9).Value = sop[index - 1].DiseaseReportingUnit.Name;
                        worksheet.Cell(index + 1, 10).Value = sop[index - 1].PcrResult == "none"? "PROCESSING" : sop[index - 1].PcrResult;
                        worksheet.Cell(index + 1, 11).Value = sop[index - 1].DatetimeCollection.GetDate("dd-MMM-yyyy hh:mm tt");
                        worksheet.Cell(index + 1, 12).Value = sop[index - 1].RequestedBy;
                        worksheet.Cell(index + 1, 13).SetDataType(XLDataType.Text);
                        worksheet.Cell(index + 1, 13).Value = sop[index - 1].RequesterContact;
                        worksheet.Cell(index + 1, 14).Value = sop[index - 1].TypeSpecimen;
                        worksheet.Cell(index + 1, 15).Value = sop[index - 1].DatetimeSpecimenReceipt == default? "PROCESSING" : sop[index - 1].DatetimeSpecimenReceipt.GetDate("dd-MMM-yyyy hh:mm tt");
                        worksheet.Cell(index + 1, 16).Value = sop[index - 1].DateResult == default ? "PROCESSING" : sop[index - 1].DateResult.GetDate("dd-MMM-yyyy hh:mm tt");
                    }
                    foreach(var item in worksheet.ColumnsUsed())
                    {
                        item.AdjustToContents();
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        /*public IActionResult Released()
        {

        }
        public IActionResult Processing()
        {

        }*/
        #endregion
        #region HELPERS

        public Task<Patient> SetPatient(AddPatientModel model)
        {
            var patient = new Patient
            {
                Fname = model.Fname,
                Mname = model.Mname,
                Lname = model.Lname,
                Sex = model.Sex,
                Dob = model.Dob,
                ContactNo = model.ContactNo,
                CurrentBarangay = model.CurrentBarangay,
                CurrentMuncity = model.CurrentMuncity,
                CurrentProvince = 2,
                CurrentPurok = model.CurrentPurok,
                CurrentSitio = model.CurrentSitio,
                CurrentAddress = model.CurrentAddress,
                PermanentBarangay = model.Disabled ? model.CurrentBarangay : model.PermanentBaragnay,
                PermanentMuncity = model.Disabled ? model.CurrentMuncity : model.PermanentMuncity,
                PermanentProvince = model.Disabled ? model.CurrentProvince : 2,
                PermanentAddress = model.Disabled ? model.CurrentAddress : model.PermanentAddress,
                PermanentPurok = model.Disabled ? model.CurrentPurok : model.PermanentPurok,
                PermanentSitio = model.Disabled ? model.CurrentSitio : model.PermanentSitio,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return Task.FromResult(patient);
        }

        public Task<Sopform> SetSopform(AddSopModel model)
        {
            var rf = new List<ResultForm>();
            rf.Add(NewResultForm());
            var form = new Sopform
            {
                PatientId = model.PatientId,
                PcrResult = "none",
                SampleId = "none",
                DateResult = default,
                DatetimeSpecimenReceipt = default,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DiseaseReportingUnitId = UserFacility,
                DateOnsetSymptoms = model.DateOnsetSymptoms?? default,
                DatetimeCollection = model.DTCollection?? default,
                RequestedBy = model.RequestedBy,
                RequesterContact = model.RequitionerContactNo,
                Swabber = model.Swabber,
                TypeSpecimen = model.SpecimenType,
                ResultForm = rf
            };

            return Task.FromResult(form);
        }

        public ResultForm NewResultForm()
        {
            var form = new ResultForm
            {
                PerformedBy = null,
                VerifiedBy = null,
                ApprovedBy = null,
                CreatedBy = 10,
                DateTimeSampleArrived = default,
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
