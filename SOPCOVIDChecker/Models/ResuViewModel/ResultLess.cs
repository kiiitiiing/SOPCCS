using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ResuViewModel
{
    public partial class ResultLess
    {
        public int ResultFormId { get; set; }
        public int SOPId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string SampleId { get; set; }
        public DateTime SampleTaken { get; set; }
        public string DRU { get; set; }
        public string PCRResult { get; set; }
    }
}
