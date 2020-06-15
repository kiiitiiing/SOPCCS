using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SOPCOVIDChecker.Data;

namespace SOPCOVIDChecker.Controllers
{
    public class HelperController : Controller
    {
        private readonly SOPCCContext _context;

        public HelperController(SOPCCContext context)
        {
            _context = context;
        }

        public partial class SelectAddress
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        [HttpGet]
        public List<SelectAddress> FilteredBarangay(int? muncityId)
        {
            var filteredBarangay = _context.Barangay
                .Where(x => x.Muncity.Equals(muncityId))
                .Select(y => new SelectAddress
                {
                    Id = y.Id,
                    Description = y.Description
                }).ToList();

            return filteredBarangay;
        }
    }
}
