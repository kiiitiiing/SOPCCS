using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.MobileModels
{
    public class MainUserModel
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string UserLevel { get; set; }
        public string Facility { get; set; }
        public int DiseaseReportingUnitId { get; set; }
    }
}
