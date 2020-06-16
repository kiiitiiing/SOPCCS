using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOPCOVIDChecker.Data;

namespace SOPCOVIDChecker.Controllers
{
    [Authorize(Policy = "PESUUsers")]
    public class PesuController : Controller
    {
        private readonly SOPCCContext _context;

        public PesuController(SOPCCContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
