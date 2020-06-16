using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    }
}
