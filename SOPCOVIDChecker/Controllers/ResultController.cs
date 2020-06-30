using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOPCOVIDChecker.Data;
using SOPCOVIDChecker.Models;
using SOPCOVIDChecker.Models.AdminViewModel;
using SOPCOVIDChecker.Models.ResultViewModel;
using SOPCOVIDChecker.Models.ResuViewModel;
using SOPCOVIDChecker.Services;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "LABUsers")]
    public class ResultController : Controller
    {
        private readonly SOPCCContext _context;
        private readonly IUserService _userService;
        private IConverter _converter;

        public ResultController(SOPCCContext context, IUserService userService, IConverter converter)
        {
            _context = context;
            _userService = userService;
            _converter = converter;
        }

        #region DASHBOARD
        public async Task<ActionResult<List<ResultLess>>> LabJson(string q)
        {
            var form = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    DRU = x.SopForm.DiseaseReportingUnit.Name,
                    PCRResult = x.SopForm.PcrResult,
                    SampleId = x.SopForm.SampleId,
                    SampleTaken = x.SopForm.DatetimeCollection,
                    Approved = x.ApprovedBy != null
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(q))
            {
                form = form.Where(x => x.PatientName.Contains(q, StringComparison.OrdinalIgnoreCase) || x.SampleId.Equals(q)).ToList();
            }

            return form;
        }
        public IActionResult LabIndex()
        {
            return View();
        }
        public IActionResult LabIndexPartial([FromBody]IEnumerable<ResultLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region VIEW RESULT FORM
        public async Task<IActionResult> ViewResultForm(int resultId)
        {
            var sop = await SetResultForm(resultId);

            ViewBag.Perform = await GetStaff("perform");
            ViewBag.Verify = await GetStaff("verify");
            ViewBag.Approve = await GetStaff("approve");

            return PartialView(sop);
        }
        #endregion
        #region RESULT FORM
        public async  Task<IActionResult> ResultForm(int resultId)
        {
            var sop = await SetResultForm(resultId);

            ViewBag.Perform = await GetStaff("perform");
            ViewBag.Verify = await GetStaff("verify");
            ViewBag.Approve = await GetStaff("approve");

            return PartialView(sop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResultForm(ResultFormModel model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);

            ViewBag.Perform = await GetStaff("perform");
            ViewBag.Verify = await GetStaff("verify");
            ViewBag.Approve = await GetStaff("approve");

            var sop = await SetResultForm(model.Id);

            if (ModelState.IsValid)
            {
                _context.Update(await GetResultForm(model));
                await _context.SaveChangesAsync();

                
                return PartialView(sop);
            }

            
            ViewBag.Errors = errors;
            return PartialView(sop);
        }
        #endregion
        #region ADD STAFF
        public IActionResult AddStaff()
        {
            ViewBag.Facilities = new SelectList(_context.Facility.ToList(), "Id", "Name");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStaff(LabUsersModel model)
        {
            ViewBag.Facilities = new SelectList(_context.Facility.ToList(), "Id", "Name");
            model.FacilityId = UserFacility;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (await _userService.RegisterLabUserAsync(model))
                {
                    return PartialView(model);
                }
            }

            ViewBag.Errors = errors;
            return PartialView(model);
        }
        #endregion
        #region LAB STAFF
        public async Task<ActionResult<List<UserLess>>> LabUsersJson(string q)
        {
            var rhuUsers = await _context.Sopusers
                .Include(x => x.Facility)
                .Include(x => x.BarangayNavigation)
                .Include(x => x.MuncityNavigation)
                .Include(x => x.ProvinceNavigation)
                .Where(x=>x.UserLevel == "perform" || x.UserLevel == "verify" || x.UserLevel == "approve")
                .Where(x => x.FacilityId == UserFacility)
                .Select(x => new UserLess
                {
                    Id = x.Id,
                    Name = x.GetFullName(),
                    ContactNo = x.ContactNo,
                    Email = x.Email,
                    Designation = x.Designation,
                    Facility = x.Facility.Name
                })
                .ToListAsync();

            return rhuUsers;
        }
        public IActionResult LabUsers()
        {
            return View();
        }

        public IActionResult LabUsersPartial([FromBody]IEnumerable<UserLess> model)
        {
            return PartialView(model);
        }
        #endregion
        #region PDF
        public async Task<IActionResult> ResultPdf(int resultId)
        {
            var result = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.Patient)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.PerformedByNavigation)
                .Include(x => x.VerifiedByNavigation)
                .Include(x => x.ApprovedByNavigation)
                .SingleOrDefaultAsync(x => x.Id == resultId);
            var file = SetResultPDF(result);
            return File(file, "application/pdf");
        }

        #region SOP COVID CHECKER RESULT PDF
        public byte[] SetResultPDF(ResultForm model)
        {
            new CustomAssemblyLoadContext().LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.Letter,
                Margins = new MarginSettings { Top = 1.27, Bottom = 1.27, Left = 1.27, Right = 1.27, Unit = Unit.Centimeters },
                DocumentTitle = "Result"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = ResultPdfHtml(model),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "css", "bootstrap.min.css") }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return _converter.Convert(pdf);
        }

        public string ResultPdfHtml(ResultForm model)
        {
            var pdf = new StringBuilder();
            var sex = model.SopForm.Patient.Sex[0].ToString().ToUpper();
            var admissionDate = model.AdmissionDate == null ? "N/A" : ((DateTime)model.AdmissionDate).ToString("MM/dd/yyyy");
            #region opening Html & divRow
            var openingHtml = "<html>" +
                                "<head>" +
                                        "<style>" +
                                                "* { box-sizing: border-box;}" +
                                                "td { padding: 3px; font-size:17; width:17%}" +
                                                ".td2 { padding: 3px; font-size:17; width:7%}" +
                                                "th {  padding: 5px; font-size:18 }" +
                                                "table { border:1px solid black;  border-spacing: 0px; width: 100%; }" + //
                                                ".footer{position:absolute;bottom:0;width:100%;}" +
                                                ".header1 {padding: 0px; font-family:Times New Roman; font-size:15;text-align:center; border:0px} " +
                                                ".BTop{border-Top:1px solid black;}" +
                                                ".BLeft{border-Left:1px solid black;}" +
                                                ".BRight{border-Right:1px solid black;}" +
                                                ".BBottom{border-Bottom:1px solid black;}" +
                                                ".pad0{padding:0px}" +
                                                ".padTop{padding-top:10px}" +
                                                ".padbottom{padding-bottom:10px}" +
                                           "</style>" +
                                "</head>" +
                                "<body>" +
                                "<div class='row' style='font-family:Calibri'>"+
                                "<div><img style='position: absolute; bottom: 48; right: 28;' width='33%' height='7%' src='https://localhost:44385/assets/dist/img/signature.png' /></div>";
            #endregion
            var header = "<table style='border-Bottom:0px'>" +
                        "<tr>"+
                            "<td class='header1'></td>"+
                            "<td class='header1'></td>"+
                            "<td class='header1'></td>"+
                            "<td class='header1'></td>"+
                            "<td class='header1'></td>"+
                            "<td class='header1'></td>"+
                        "</tr>" +
                        "<tr style='padding-top:5px'>" +
                            "<td  class='header1 BBottom' rowspan='7' style='border-right:1px solid black; padding-left:10 !importantx;'><img style='display:block;' width='90%' height='40%' src='https://localhost:44385/assets/dist/img/RLMF.png' /></td>" + //INSERT LOGO
                            "<td  class='header1' colspan='4' style='padding-top:10px' >Republic of the Philippines</td>" +
                            "<td  class='header1' rowspan='2' style='border-left:1px solid black; padding-top:10px'>DOH-CTRL-MF-F-001-A</td>" + //DOH-CTRL-MF-F-001-A
                        "</tr>" +
                        "<tr>" +
                            "<td class='header1' colspan='4' >Department of Health</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td class='header1' colspan='4' >CENTRAL VISAYAS CENTER FOR HEALTH DEVELOPMENT</td>" +
                            "<td class='header1' rowspan='2' style='border-left:1px solid black'>Revision No. 2 </td>" + // Revision No. 2
                        "</tr>" +
                        "<tr>" +
                            "<td class='header1' colspan='4'>Cebu TB Reference Laboratory</td>" +
                        "</tr>" +
                         "<tr>" +
                            "<td class='header1' colspan='4' style='font-weight:bold; font-style:italic'>Molecular Facility for COVID-19 Testing</td>" +
                             "<td  class='header1' rowspan='2' style='border-left:1px solid black'>Page 1 of 1 </td>" + //Page 1 of 1 
                        "</tr>" +
                        "<tr>" +
                            "<td class='header1' colspan='4'>Osmeña Boulevard, Sambag II, Cebu City 6000</td>" +
                        "</tr>" +
                        "<tr> <td class='header1 BBottom' colspan='6'></td> </tr>";

            var patient = "<tr>" +
                              "<td><b>Name of Patient: </b></td>" +
                              "<td colspan='5' style='color: #dc3545;'> " + model.SopForm.Patient.GetFullName().ToUpper()+" </td>" +//patient Name
                          "</tr>" +
                           "<tr>" +
                              "<td colspan='6' class='BBottom pad0'> &nbsp; &nbsp; <i> Last Name, First Name, Middle Name</i> </td>" +
                          "</tr>" +
                           "<tr>" +
                                "<td class='BBottom' rowspan='2'><b>Age: </b>  &nbsp; &nbsp;<span style='color: #dc3545;'>" + model.SopForm.Patient.Dob.ComputeAge() + "</span></td>" + //age
                                "<td colspan='2' rowspan='2'class='BBottom BRight'> &nbsp; &nbsp; &nbsp; &nbsp;<b>Sex: </b>&nbsp; &nbsp;<span style='color: #dc3545;'>" + sex + "</span></td>" +//Sex
                                "<td><b> Date of Birth: </b></td>" +
                                "<td colspan='2' style='color: #dc3545;'>" + model.SopForm.Patient.Dob.GetDate("dd/MM/yyyy") + "</td>" + //Date of Birth
                          "</tr>" +
                          "<tr>" +
                            "<td colspan='3' class='BBottom pad0'>&nbsp; <i>dd/mm/yyyy</i></td>" + //Sample ID
                          "</tr>" +
                          "<tr>" +
                            "<td><b>Patient Location: </b></td>" +
                            "<td colspan='2' class='BRight'></td>" +//patient Location
                            "<td><b> Sample ID: </b></td>" +
                            "<td colspan='2' style='color: #dc3545;'>" + model.SopForm.SampleId + "</td>" + //Sample ID
                          "</tr>" +
                          "<tr>" +
                            "<td>RHU/CHO</td>" +
                            "<td colspan='2' class='BRight'></td>" +//RHU/CHO
                            "<td colspan='3' class='BBottom pad0'></td>" +
                          "</tr>" +
                           "<tr>" +
                            "<td>Hospital/Infirmary</td>" +
                            "<td colspan='2' rowspan='2' class='BRight BBottom' style='color: #dc3545;'> " + model.SopForm.DiseaseReportingUnit.Name+" </td>" +// Hospital/Infirmary/referral
                            "<td colspan='1'><b>Admission Date</b></td>" +
                            "<td colspan='2' style='color: #dc3545;'> " + admissionDate + " </td>" + //Admission Date
                          "</tr>" +
                           "<tr>" +
                            "<td class='BBottom'>Referral:</td>" +
                            "<td colspan='3' class='BBottom pad0'>&nbsp; <i>dd/mm/yyyy</i></td>" +
                          "</tr>" +
                          "<tr>" +
                              "<td class='BBottom'><b>Requisitioner:</b></td>" +
                              "<td class='BBottom BRight' colspan='2' style='color: #dc3545;'>" + model.SopForm.RequestedBy + "</td>" + // Requisitioner
                              "<td><b>Address:</b></td>" +
                              "<td class='BBottom' colspan='2' rowspan='2' style='color: #dc3545;'>" + model.SopForm.Patient.GetAddress() + "</td>" + //ADDRESS
                          "</tr>" +
                          "<tr>" +
                              "<td class='BBottom'><b>Specimen Type:</b></td>" +
                              "<td class='BBottom BRight' colspan='2' style='color: #dc3545;'>" + model.SopForm.TypeSpecimen + "</td>" + // Specimen Type
                               "<td class='BBottom' colspan='2'></td>" +
                          "</tr>" +
                           "<tr>" +
                              "<td colspan='2'><b>Date & Time of Specimen Collection:</b></td>" +
                              "<td rowspan='2' class='BBottom BRight' style='color: #dc3545;'>" + model.SopForm.DatetimeCollection.GetDate("dd/MM/yyyy") +" <br> " + model.SopForm.DatetimeCollection.GetDate("HH:mm") + "</td>" + // Date & Time of Specimen Collection
                              "<td colspan='2'> <b>Date & Time of Specimen Receipt:</b></td>" +
                              "<td rowspan='2'  class='BBottom' style='color: #dc3545;'>" + model.SopForm.DatetimeSpecimenReceipt.GetDate("dd/MM/yyyy") + " <br> " + model.SopForm.DatetimeSpecimenReceipt.GetDate("HH:mm") + "</td>" + //// Date & Time of Specimen Receipt
                          "</tr>" +
                           "<tr>" +
                              "<td colspan='2' class='pad0 BBottom'>&nbsp; <i>dd/mm/yyyy hh:mm</i></td>" +
                              "<td colspan='2' class='pad0 BBottom'>&nbsp; <i>dd/mm/yyyy hh:mm</i></td>" +
                           "</tr>" +
                           "<tr>" +
                              "<td colspan='2'><b>Date & Time of Release Result:</b></td>" +
                              "<td rowspan='2' class='BBottom BRight' style='color: #dc3545;'>" + model.SopForm.DateResult.GetDate("dd/MM/yyyy") + " <br> " + model.SopForm.DateResult.GetDate("HH:mm") + "</td>" + // Date & Time of Release Result
                          "</tr>" +
                           "<tr>" +
                              "<td colspan='2' class='pad0 BBottom'>&nbsp; <i>dd/mm/yyyy hh:mm</i></td>" +
                               "<td colspan='3' class='pad0 BBottom'></td>" +
                           "</tr>" +
                           "<tr><td colspan='6'></td></tr>";

            var labResult = "<tr><td colspan='6' style='text-align:center' class='BBottom'><b>LABORATORY TEST RESULT</b></td></tr>" +
                    "<tr >" +
                        "<td colspan='2' class='padTop' > LABORATORY TEST PERFORMED: </td>" +
                        "<td colspan='3' rowspan='2' class='padTop'>" + " SARS-CoV-2 (causative agent of COVID-19) virus detection by Real-Time Polymerase Chain Reaction" + "</td>" + //TEST PERFORMED
                    "</tr>" +
                     "<tr><td></td></tr>" +
                     "<tr>" +
                        "<td colspan='2' class='padTop'> TEST RESULT :</td>" +
                        "<td colspan='4' class='padTop' style='color: #dc3545; font-weight: 600;'> " + model.SopForm.PcrResult + " </td>" + //TEST RESULT
                    "</tr>" +
                    "<tr>" +
                        "<td colspan='2' class='padTop'> TEST RESULT AND UNITS OR MEASUREMENTS:</td>" +
                        "<td colspan='4' class='padTop'><b>" + "None" + "</b></td>" + //TEST RESULT AND UNITS OR MEASUREMENTS
                    "</tr>" +
                    "<tr>" +
                    "<td colspan='2'> BIOLOGICAL REFERENCE INTERVALS:</td>" +
                        "<td colspan='4'><b> " + "None" + " </b></td>" + //BIOLOGICAL REFERENCE INTERVALS
                    "</tr>" +
                    "<tr ><td colspan='6' style='text-align:center' class='padTop'> INTERPRETATION OF RESULTS WHEN APPROPRIATE: </td></tr> " +
                "</table>";

            var resultTable = "<table style='border-Top:0px'>" +
                "<tr>  <td class='td2'></td> <td class='td2'></td> <td class='td2'></td> <td class='td2'></td ><td class='td2'></td> <td class='td2'></td> <td class='td2'></td> " +
                "<td class='td2'></td> <td class='td2'></td> <td class='td2'></td> <td class='td2'></td ><td class='td2'></td> <td class='td2'></td> <td class='td2'></td> </tr>" +
                "<tr>" +
                    "<td class='td2'></td>" +
                    "<td colspan='6' class='BTop BBottom BLeft BRight' style='text-align:center' >FINAL RESULT</td>" +
                    "<td colspan='6' class='BTop BBottom BRight'style='text-align:center' >INTERPRETATION</td>" +
                    "<td></td>" +
                "</tr>" +
                 "<tr>" +
                    "<td class='td2'></td>" +
                    "<td colspan='6' class=' BBottom BLeft BRight'>SARS-CoV-2 (causative agent of COVID-19) viral RNA detected</td>" +
                    "<td colspan='6' class=' BBottom BRight'>Positive for SARS-CoV-2 (causative agent of COVID-19)</td>" +
                    "<td></td>" +
                "</tr>" +
                 "<tr>" +
                    "<td class='td2'></td>" +
                    "<td colspan='6' class=' BBottom BLeft BRight'>SARS-CoV-2 (causative agent of COVID-19) viral RNA not detected</td>" +
                    "<td colspan='6' class=' BBottom BRight'>Negative for SARS-CoV-2 (causative agent of COVID-19)</td>" +
                    "<td></td>" +
                "</tr>" +
                 "<tr>" +
                "<td class='td2'></td>" +
                    "<td colspan='6' class=' BBottom BLeft BRight'>Invalid due to specimen quality	</td>" +
                    "<td colspan='6' class=' BBottom BRight'>Negative for test internal control (most likely due to poor specimen quality)	</td>" +
                    "<td class='td2'></td>" +
                "</tr>" +
                "<tr><td colspan='14' style='text-align:center' class='padTop padBottom'><i><b>This laboratory result should be interpreted together with the available clinical and epidemiological information.</b></i></td></tr> " +

                "<tr><td colspan='14' class='BTop'></td></tr>" +
                "<tr>" +
                    "<td class='td2 ' colspan='2'><b>COMMENTS:</b></td>" +
                    "<td class='td2 BBottom' colspan='12' rowspan='5' style='color: #dc3545;'>" + model.Comments + "</td>" + //Comments

                "</tr>" +
               "<tr><td class='td2' height='20px'></td> </tr>" +
               "<tr><td class='td2' height='20px'></td> </tr>" +
               "<tr><td class='td2' height='20px'></td> </tr>" +
               "<tr><td colspan='2' class='td2 BBottom' height='20px'></td> </tr>";

            var signature = "<tr class='BTop'>" +
                                 "<td class='td2' colspan='4'><b>Performed:</b></td>" +
                                 "<td class='td2'></td>" +
                                 "<td class='td2' colspan='4'><b>Verified:</b></td>" +
                                 "<td class='td2'></td>" +
                                 "<td class='td2' colspan='4'><b>Approved:</b></td>" +
                            " </tr>" +
                            "<tr>" +
                                 "<td class='td2' colspan='4' rowspan='3'></td>" + //Performed Signature 
                                 "<td class='td2'></td>" +
                                 "<td class='td2' colspan='4' rowspan='3'></td>" +//Verified Signature 
                                 "<td class='td2'></td>" +
                                 "<td class='td2' colspan='4' rowspan='3'></td>" +//Approved Signature 
                            " </tr>" +
                            "<tr><td class='td2'></td> </tr>" +
                            "<tr><td class='td2'></td> </tr>" +
                            "<tr><td class='td2'></td> </tr>" +
                            "<tr>" +
                                "<td class='td2' colspan='4' style='text-align:center'><u><b>" + model.PerformedByNavigation.GetFullName().ToUpper()+","+model.PerformedByNavigation.Postfix + "</b></u></td> " + //MEDICAL TECHNOLOGIST
                                "<td class='td2'></td>" +
                                "<td class='td2' colspan='4' style='text-align:center'><u><b>" + model.VerifiedByNavigation.GetFullName().ToUpper() + "," + model.VerifiedByNavigation.Postfix + "</b></u></td> " + // LABORATORY SUPERVISOR

                                "<td class='td2' colspan='5' style='text-align:center'><u><b>" + model.ApprovedByNavigation.GetFullName().ToUpper() + "," + model.ApprovedByNavigation.Postfix + "</b></u></td> " + //CHIEF PATHOLOGIST
                            "</tr>" +
                            "<tr>" +
                                "<td class='td2' colspan='4' style='text-align:center'>" + model.PerformedByNavigation.Designation + "</td> " +
                                "<td class='td2'></td>" +
                                "<td class='td2' colspan='4' style='text-align:center'>" + model.VerifiedByNavigation.Designation + "</td> " +

                                "<td class='td2' colspan='4' style='text-align:center'>" + model.VerifiedByNavigation.Designation + "</td> " +
                            "</tr>" +
                            "<tr>" +
                                "<td class='td2 pad0' colspan='4' style='text-align:center'>Lic No. &nbsp;" + model.PerformedByNavigation.LicenseNo + "</td> " + //MEDICAL TECHNOLOGIST Lic. No.
                                "<td class='td2 pad0'></td>" +
                                "<td class='td2 pad0' colspan='4' style='text-align:center'>Lic No. &nbsp;" + model.VerifiedByNavigation.LicenseNo + " </td> " + // LABORATORY SUPERVISOR Lic. No.

                                "<td class='td2 pad0' colspan='4' style='text-align:center' >Lic No. &nbsp;" + model.ApprovedByNavigation.LicenseNo + "</td> " + //CHIEF PATHOLOGIST Lic. No.
                            "</tr>";

            var closingTags = "</table></div></div><!--row end--> </body></html>";

            pdf.Append(openingHtml);
            pdf.Append(header);
            pdf.Append(patient);
            pdf.Append(labResult);
            pdf.Append(resultTable);
            pdf.Append(signature);

            pdf.Append(closingTags);
            return pdf.ToString();
        }
        #endregion
        #endregion
        #region HELPERS

        public async Task<ResultFormModel> SetResultForm(int resultId)
        {
            var model = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x=>x.PerformedByNavigation)
                .Include(x=>x.VerifiedByNavigation)
                .Include(x => x.ApprovedByNavigation)
                .SingleOrDefaultAsync(x => x.Id == resultId);

            var rfm = new ResultFormModel
            {
                Id = model.Id,
                SopFormId = model.SopFormId,
                Name = model.SopForm.Patient.GetFullName().ToUpper(),
                Age = model.SopForm.Patient.Dob.ComputeAge(),
                DoB = model.SopForm.Patient.Dob,
                Sex = model.SopForm.Patient.Sex,
                Address = model.SopForm.Patient.GetAddress(),
                Requisitioner = model.SopForm.RequestedBy,
                SpecimenType = model.SopForm.TypeSpecimen,
                Location = model.SopForm.DiseaseReportingUnit.Name,
                AdmissionDate = model.AdmissionDate,
                SampleArrived = model.DateTimeSampleArrived == default ? null : (DateTime?)model.DateTimeSampleArrived,
                DTSpecimeCollection = model.SopForm.DatetimeCollection,
                DTSpecimenReceipt = model.SopForm.DatetimeSpecimenReceipt == default ? DateTime.Now.RemoveSeconds() : model.SopForm.DatetimeSpecimenReceipt.RemoveSeconds(),
                DTReleaseResult = model.SopForm.DateResult == default ? DateTime.Now.RemoveSeconds() : model.SopForm.DateResult.RemoveSeconds(),
                TestResult = model.SopForm.PcrResult,
                SampleID = model.SopForm.SampleId.Equals("none") ? "" : model.SopForm.SampleId,
                Performed = model.PerformedBy,
                Verified = model.VerifiedBy,
                Approved = model.ApprovedBy,
                Comments = model.Comments,
                PerformedBy = model.PerformedByNavigation == null ? "" : model.PerformedByNavigation.GetFullName(),
                VerifiedBy = model.VerifiedByNavigation == null ? "" : model.VerifiedByNavigation.GetFullName(),
                ApprovedBy = model.ApprovedByNavigation == null ? "" : model.ApprovedByNavigation.GetFullName()
            };

            return rfm;
        }

        public async Task<ResultForm> GetResultForm(ResultFormModel model)
        {
            var resultForm = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentBarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentMuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.CurrentProvinceNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .SingleOrDefaultAsync(x => x.Id == model.Id);

            resultForm.SopForm.SampleId = model.SampleID;
            resultForm.AdmissionDate = model.AdmissionDate;
            resultForm.SopForm.DatetimeSpecimenReceipt = model.DTSpecimenReceipt;
            resultForm.SopForm.DateResult = model.DTReleaseResult;
            resultForm.SopForm.PcrResult = model.TestResult;
            resultForm.Comments = model.Comments;
            resultForm.PerformedBy = model.Performed;
            resultForm.VerifiedBy = model.Verified;
            resultForm.ApprovedBy = model.Approved;
            resultForm.UpdatedAt = DateTime.Now;
            resultForm.SopForm.UpdatedAt = DateTime.Now;
            resultForm.DateTimeSampleArrived = (DateTime)model.SampleArrived;

            return resultForm;
        }

        public partial class SelectUser
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public async Task<SelectList> GetStaff(string role)
        {
            var staff = await _context.Sopusers
                .Where(x => x.FacilityId == UserFacility && x.UserLevel == role)
                .Select(x => new SelectUser
                {
                    Id = x.Id,
                    Name = x.GetFullName()
                })
                .ToListAsync();
            return new SelectList(staff, "Id", "Name");
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
