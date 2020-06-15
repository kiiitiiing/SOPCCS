using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SOPCOVIDChecker.Data;

namespace SOPCOVIDChecker.Controllers
{
    public class FormsController : Controller
    {
        private readonly SOPCCContext _context;

        public FormsController(SOPCCContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region HELPERS

        public int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        public int UserFacility => int.Parse(User.FindFirstValue("Facility"));
        public int UserProvince => int.Parse(User.FindFirstValue("Province"));
        public int UserMuncity => int.Parse(User.FindFirstValue("Muncity"));
        public int UserBarangay => int.Parse(User.FindFirstValue("Barangay"));
        public string UserName => User.FindFirstValue(ClaimTypes.GivenName) + " " + User.FindFirstValue(ClaimTypes.Surname);

        #endregion
    }
}
