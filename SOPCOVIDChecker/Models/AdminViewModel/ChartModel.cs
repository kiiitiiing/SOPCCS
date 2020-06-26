using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.AdminViewModel
{
    public partial class ChartModel
    {
        public List<string> Day { get; set; }
        public List<int> Positive { get; set; }
        public List<int> Negative { get; set; }
        public List<int> Invalid { get; set; }
        public List<int> Processing { get; set; }
    }
}
