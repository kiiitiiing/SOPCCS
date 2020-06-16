using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOPCOVIDChecker.Data;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "RESUUsers")]
    public class ResuController : Controller
    {
        private readonly SOPCCContext _context;

        public ResuController(SOPCCContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
