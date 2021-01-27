using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.CovidKayaModels;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Models.SopViewModel;
using SOPCOVIDChecker.Services;
using CellType = NPOI.SS.UserModel.CellType;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "RHUUsers")]
    public class SopController : Controller
    {
        private readonly SOPCCContext _context;
        private IWebHostEnvironment _hostingEnvironment;


        public SopController(SOPCCContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region DOWNLOAD LINE LIST FORM
        public async Task<IActionResult> LineListForm()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "assets", "dist", "apk", "LineList_Template.xlsx");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "APPLICATION/octet-stream", Path.GetFileName(path));
        }
        #endregion

        #region UPLOAD EXCEL
        public IActionResult UploadLineList()
        {
            return View();
        }
        public async Task<ActionResult> Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            List<Patient> employees = new List<Patient>();
            StringBuilder sb = new StringBuilder();
            var errors = false;
            var ctr = 0;
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<div class='callout callout-danger'>");
                    sb.Append("<h4>Please check for errors!</h4>");
                    sb.Append("</div>");
                    sb.Append("<table class='table table-hover table-bordered table-responsive table-striped' style='white-space: nowrap!important'>");
                    sb.Append("<thead class='bg-gray'>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th class='text-center text-sm'>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</thead>");
                    sb.Append("<tbody>");
                    var forms = new List<Sopform>();
                    var sopForms = await _context.Sopform.ToListAsync();
                    var provinces = await _context.Province.ToListAsync();
                    var muncities = await _context.Muncity.ToListAsync();
                    var barangays = await _context.Barangay.ToListAsync();
                    var facilities = await _context.Facility.ToListAsync();
                    if(cellCount == 20)
                    {
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                        {
                            var form = new Sopform();
                            var patient = new Patient();
                            sb.Append("<tr>");
                            IRow row = sheet.GetRow(i);
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    if (j == 0)//specimen id
                                    {
                                        if (sopForms.Any(x => x.SampleId.ToUpper() == row.GetCell(j).ToString().ToUpper()) || string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                        {
                                            sb.Append("<td class='text-center text-danger text-sm'>Sample ID duplicate</td>");
                                            errors = true;
                                        }
                                        else
                                        {
                                            form.SampleId = row.GetCell(j).ToString().ToUpper();
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString().ToUpper() + "</td>");
                                        }
                                    }
                                    else if (j == 1)//last name
                                    {
                                        patient.ContactNo = "N/A";
                                        patient.Lname = row.GetCell(j).ToString();
                                        sb.Append("<td class='text-center text-sm'>" + patient.Lname + "</td>");
                                    }
                                    else if (j == 2)//first name
                                    {
                                        patient.Fname = row.GetCell(j).ToString();
                                        sb.Append("<td class='text-center text-sm'>" + patient.Fname + "</td>");
                                    }
                                    else if (j == 3)//middle name
                                    {
                                        patient.Mname = row.GetCell(j).ToString();
                                        sb.Append("<td class='text-center text-sm'>" + patient.Mname + "</td>");
                                    }
                                    else if (j == 4)//birthdate
                                    {
                                        if (DateTime.TryParseExact(row.GetCell(j).ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                        {
                                            patient.Dob = result;
                                            sb.Append("<td class='text-center text-sm'>" + result.ToString("MM/dd/yyyy") + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid DateTime Format (mm/dd/yyyy)</td>");
                                        }
                                    }
                                    else if (j == 5)//sex
                                    {
                                        string sex = row.GetCell(j).ToString();
                                        if (sex == "Male" || sex == "Female")
                                        {
                                            patient.Sex = sex;
                                            sb.Append("<td class='text-center text-sm'>" + sex + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Please follow format (Male/Female)</td>");
                                        }
                                    }
                                    else if (j == 6)//province
                                    {
                                        var province = provinces.FirstOrDefault(x => x.Description.ToUpper().Contains(row.GetCell(j).ToString().ToUpper()));
                                        if (province != null)
                                        {
                                            patient.CurrentProvince = province.Id;
                                            patient.PermanentProvince = province.Id;
                                            patient.CurrentAddress = "N/A";
                                            patient.PermanentAddress = "N/A";
                                            sb.Append("<td class='text-center text-sm'>" + province.Description + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Province Does not exist in Database</td>");
                                        }
                                    }
                                    else if (j == 7)//muncity
                                    {
                                        var muncity = muncities.Where(x => x.Province == patient.CurrentProvince).FirstOrDefault(x => x.Description.ToUpper().Contains(row.GetCell(j).ToString().ToUpper()));
                                        if (muncity != null)
                                        {
                                            patient.CurrentMuncity = muncity.Id;
                                            patient.PermanentMuncity = muncity.Id;
                                            sb.Append("<td class='text-center text-sm'>" + muncity.Description + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>City/Municipality Does not exist in Database</td>");
                                        }
                                    }
                                    else if (j == 8)//barangay
                                    {
                                        var barangay = barangays
                                            .Where(x => x.Province == patient.CurrentProvince && x.Muncity == patient.CurrentMuncity)
                                            .FirstOrDefault(x => x.Description.ToUpper().Contains(row.GetCell(j).ToString().ToUpper()));
                                        if (barangay != null)
                                        {
                                            patient.CurrentBarangay = barangay.Id;
                                            patient.PermanentBarangay = barangay.Id;
                                            sb.Append("<td class='text-center text-sm'>" + barangay.Description + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Barangay Does not exist in Database</td>");
                                        }
                                    }
                                    else if (j == 9)//DRU
                                    {
                                        var facility = facilities.FirstOrDefault(x => x.Name == row.GetCell(j).ToString());
                                        if (facility != null)
                                        {
                                            form.DiseaseReportingUnitId = facility.Id;
                                            sb.Append("<td class='text-center text-sm'>" + facility.Name + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-sm'>Facility does not exists in Database</td>");
                                        }
                                    }
                                    else if (j == 10)//DateOnsetSymptoms
                                    {
                                        if (DateTime.TryParseExact(row.GetCell(j).ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                        {
                                            form.DateOnsetSymptoms = result;
                                            sb.Append("<td class='text-center text-sm'>" + result.ToString("MM/dd/yyyy") + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid DateTime Format (mm/dd/yyyy)</td>");
                                        }
                                    }
                                    else if (j == 11)//DateSpecimenCollection
                                    {
                                        if (DateTime.TryParseExact(row.GetCell(j).ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                        {
                                            form.DatetimeCollection = result;
                                            sb.Append("<td class='text-center text-sm'>" + result.ToString("MM/dd/yyyy") + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid DateTime Format (mm/dd/yyyy)</td>");
                                        }
                                    }
                                    else if (j == 12)//DateSpecimenReceived
                                    {
                                        if (DateTime.TryParseExact(row.GetCell(j).ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                        {
                                            form.DatetimeSpecimenReceipt = result;
                                            sb.Append("<td class='text-center text-sm'>" + result.ToString("MM/dd/yyyy") + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid DateTime Format (mm/dd/yyyy)</td>");
                                        }
                                    }
                                    else if (j == 13)//Date result released
                                    {
                                        if (DateTime.TryParseExact(row.GetCell(j).ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                                        {
                                            form.DateResult = result;
                                            sb.Append("<td class='text-center text-sm'>" + result.ToString("MM/dd/yyyy") + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid DateTime Format (mm/dd/yyyy)</td>");
                                        }
                                    }
                                    else if (j == 14)//specimen type
                                    {
                                        if (Helpers.SpecimenTypes.Any(x => x.Key.ToUpper() == row.GetCell(j).ToString().ToUpper()))
                                        {
                                            form.TypeSpecimen = row.GetCell(j).ToString();
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>Invalid type</td>");
                                        }

                                    }
                                    else if (j == 15)//Requested by
                                    {
                                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                        {
                                            form.RequestedBy = row.GetCell(j).ToString();
                                            form.RequesterContact = "N/A";
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>No value</td>");
                                        }
                                    }
                                    else if (j == 16)//swabber
                                    {
                                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                        {
                                            form.Swabber = row.GetCell(j).ToString();
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>No value</td>");
                                        }
                                    }
                                    else if (j == 17)//specimen number
                                    {
                                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                        {
                                            //form.SpecimenNo = row.GetCell(j).ToString();
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>No value</td>");
                                        }
                                    }
                                    else if (j == 18)//result
                                    {
                                        if (!string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                        {
                                            form.PcrResult = row.GetCell(j).ToString();
                                            sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                        }
                                        else
                                        {
                                            errors = true;
                                            sb.Append("<td class='text-center text-danger text-sm'>No value</td>");
                                        }
                                    }
                                    else if (j == 19)//remarks
                                    {
                                        //form.Remarks = row.GetCell(j).ToString();
                                        sb.Append("<td class='text-center text-sm'>" + row.GetCell(j).ToString() + "</td>");
                                    }
                                }
                            }
                            form.Patient = await CheckPatient(patient);
                            form.CreatedAt = DateTime.Now;
                            form.UpdatedAt = DateTime.Now;
                            forms.Add(form);
                            sb.Append("</tr>");
                        }
                        ctr = forms.Count();
                        sb.Append("</tbody>");
                        sb.Append("</table>");


                        if (!errors)
                        {
                            sb.Clear();
                            sb.Append("<div class='callout callout-success'>");
                            sb.Append("<h4>Upload Successfull!</h4>");
                            sb.Append("<label>Uploaded By: </label>" + UserName + "<br>");
                            sb.Append("<label>Date and Time: </label>" + DateTime.Now.GetDate("dd/MM/yyyy hh:mm:ss tt") + "<br>");
                            sb.Append("<label>No. of Records: </label>" + ctr + "<br>");
                            sb.Append("</div>");
                            _context.UpdateRange(forms);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        sb.Clear();
                        sb.Append("<div class='callout callout-danger'>");
                        sb.Append("<h4>Upload Failed!</h4>");
                        sb.Append("<p>Please use correct format.</p>");
                        sb.Append("</div>");
                        return Content(sb.ToString());
                    }
                }
            }
            return Content(sb.ToString());
        }
        #endregion

        #region DOWNLOAD
        public async Task<IActionResult> DownloadApp()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "assets","dist","apk", "com.companyname.sopcc-armeabi-v7a.apk");

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
        public IActionResult SopIndex()
        {
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;

            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SopIndexPartial(string q, string dr, int? page)
        {

            if (!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.ParseExact(dr.Substring(0, dr.IndexOf(" ") + 1).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                EndDate = DateTime.ParseExact(dr.Substring(dr.LastIndexOf(" ")).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            }
            else
            {
                var date = DateTime.Now;
                StartDate = new DateTime(date.Year, date.Month, 1);
                EndDate = DateTime.Now.Date;
            }

            var sop = _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentBarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentMuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.PermanentProvinceNavigation)
                .Include(x => x.ResultForm)
                .Include(x => x.DiseaseReportingUnit)
                .Where(x => x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                .Where(x => x.DiseaseReportingUnitId == UserFacility)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new SopLess
                {
                    SampleId = x.SampleId,
                    PatientName = x.Patient.Fname + " " + (x.Patient.Mname ?? "") + " " + x.Patient.Lname,
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
                    DateOnset = x.DateOnsetSymptoms,
                    Swabber = x.Swabber,
                    ContactNo = x.Patient.ContactNo,
                    CreatedAt = x.CreatedAt
                });

            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                sop = sop.Where(x => x.PatientName.Contains(q));
            }
            int size = 10;
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView(PaginatedList<SopLess>.CreateAsync(await sop.ToListAsync(), action, page ?? 1, size));
        }
        #endregion

        #region ADD SOP FORM
        public IActionResult AddSopModal(int patientId)
        {
            var noSpec = _context.Sopform.Where(x => x.PatientId == patientId).Count();
            var model = new AddSopModel
            {
                PatientId = patientId,
                Disabled = false,
                DateOnsetSymptoms = null,
                NumSpec = (noSpec + 1)
            };
            ViewBag.Types = new SelectList(Helpers.SpecimenTypes, "Key", "Value");
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSopModal(AddSopModel model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);
            ViewBag.Types = new SelectList(Helpers.SpecimenTypes, "Key", "Value");
            if (model.Disabled)
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
        public IActionResult SampleStatus()
        {
            ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value");
            var date = DateTime.Now;
            StartDate = new DateTime(date.Year, date.Month, 1);
            EndDate = DateTime.Now.Date;
            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SampleStatusPartial(string q, string dr, string f, int? page)
        {

            ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value");
            if (!string.IsNullOrEmpty(dr))
            {
                StartDate = DateTime.ParseExact(dr.Substring(0, dr.IndexOf(" ") + 1).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                EndDate = DateTime.ParseExact(dr.Substring(dr.LastIndexOf(" ")).Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
            }
            else
            {
                var date = DateTime.Now;
                StartDate = new DateTime(date.Year, date.Month, 1);
                EndDate = DateTime.Now.Date;
            }

            var sop = _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.UpdatedAt >= StartDate && x.UpdatedAt <= EndDate)
                .Where(x => x.SopForm.DiseaseReportingUnitId == UserFacility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    SampleId = x.SopForm.SampleId,
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.Fname + " " + (x.SopForm.Patient.Mname ?? "") + " " + x.SopForm.Patient.Lname,
                    Lab = x.CreatedByNavigation.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    Address = x.SopForm.Patient.GetAddress(),
                    Status = x.ApprovedBy == null? "PROCESSING" : "RELEASED"
                });
            if (!string.IsNullOrEmpty(q))
            {
                ViewBag.Search = q;
                sop = sop.Where(x => x.PatientName.Contains(q));
            }

            if (!string.IsNullOrEmpty(f))
            {
                ViewBag.Filters = new SelectList(Helpers.Filters, "Key", "Value", f);
                sop = sop.Where(x => x.Status.Equals(f));
            }


            ViewBag.StartDate = StartDate.Date.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.Date.ToString("dd/MM/yyyy");
            int size = 10;
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            return PartialView(PaginatedList<ResultLess>.CreateAsync(await sop.ToListAsync(), action, page ?? 1, size));
        }
        #endregion

        #region PATIENTS
        [HttpGet]
        public IActionResult Patients(string q, int? page)
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

        #region ADD PATIENT

        public IActionResult AddPatient()
        {
            ViewBag.Muncity = GetMuncities(UserProvince);
            var model = new AddPatientModel
            {
                Disabled = false,
                Employed = true
            };
            return PartialView(model);
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
            if(!model.Employed)
            {
                ModelState.Remove("PEN");
                ModelState.Remove("EmployerName");
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

        public async Task<Patient> CheckPatient( Patient patient )
        {
            var curPatient = await _context.Patient.FirstOrDefaultAsync(x =>
                x.Fname == patient.Fname &&
                x.Mname == patient.Mname &&
                x.Lname == patient.Lname &&
                x.Dob == patient.Dob &&
                x.Sex == patient.Sex);
            if (curPatient != null)
                return curPatient;
            else
                return patient;
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
                Pen = model.Employed? model.PEN : "Unemployed",
                EmployerName = model.Employed? model.EmployerName : "Unemployed",
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
                //SpecimenNo = model.NumSpec,
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
