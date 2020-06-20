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

        public ResultController(SOPCCContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        #region DASHBOARD
        public async Task<ActionResult<List<ResultLess>>> LabJson(string q)
        {
            var form = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.CreatedByNavigation).ThenInclude(x => x.Facility)
                .Where(x => x.CreatedBy != null)
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => new ResultLess
                {
                    ResultFormId = x.Id,
                    SOPId = x.SopFormId,
                    PatientId = x.SopForm.PatientId,
                    PatientName = x.SopForm.Patient.GetFullName(),
                    DRU = x.SopForm.DiseaseReportingUnit.Facility.Name,
                    PCRResult = x.SopForm.PcrResult,
                    SampleId = x.SopForm.SampleId,
                    SampleTaken = x.SopForm.DatetimeCollection
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

        #region RESULT FORM
        public async Task<IActionResult> ResultForm(int resultId)
        {
            var sop = await _context.ResultForm
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.SopForm).ThenInclude(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .SingleOrDefaultAsync(x => x.Id == resultId);

            ViewBag.Perform = GetStaff("perform");
            ViewBag.Verify = GetStaff("verify");
            ViewBag.Approve = GetStaff("approve");

            

            return PartialView(sop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResultForm(ResultForm model)
        {
            var errors = ModelState.Values.SelectMany(x => x.Errors);

            ViewBag.Perform = GetStaff("perform");
            ViewBag.Verify = GetStaff("verify");
            ViewBag.Approve = GetStaff("approve");

            if (ModelState.IsValid)
            {
                model.UpdatedAt = DateTime.Now;
                _context.Update(model);
                await _context.SaveChangesAsync();

                var sop = await _context.Sopform
                .Include(x => x.Patient).ThenInclude(x => x.BarangayNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.MuncityNavigation)
                .Include(x => x.Patient).ThenInclude(x => x.ProvinceNavigation)
                .Include(x => x.DiseaseReportingUnit).ThenInclude(x => x.Facility)
                .SingleOrDefaultAsync(x => x.Id == model.SopFormId);

                model.SopForm = sop;
                return PartialView(model);
            }

            
            ViewBag.Errors = errors;
            return PartialView(model);
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

        #region HELPERS

        public partial class SelectUser
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public SelectList GetStaff(string role)
        {
            var staff = _context.Sopusers
                .Where(x => x.FacilityId == UserFacility && x.UserLevel == role)
                .Select(x => new SelectUser
                {
                    Id = x.Id,
                    Name = x.GetFullName()
                })
                .ToList();
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
