using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.CovidKayaModels
{
    public class CKAddress
    {
        public List<string> Line { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public List<CKExtention> Extention { get; set; }
    }
}
