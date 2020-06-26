using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.CovidKayaModels
{
    public class CKTelcom
    {
        public string System { get; set; }
        public string Value { get; set; }
        public List<CKExtention> Extention { get; set; }
    }
}
