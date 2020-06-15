using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.SopViewModel
{
    public partial class SopLess
    {
        public string SampleId { get; set; }
        public string PatientName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PCRResult { get; set; }
        public string DRU { get; set; }
        public string Address { get; set; }
        public DateTime DateTimeCollection { get; set; }
        public string RequestedBy { get; set; }
        public string RequesterContact { get; set; }
        public string SpecimenCollection { get; set; }
        public DateTime DateTimeReceipt { get; set; }
        public DateTime DateResult { get; set; }
    }
}
