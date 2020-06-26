using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.CovidKayaModels
{
    public class CKPatient
    {
        public string ResourceType { get; set; }
        public string Id { get; set; }
        public List<CKName> Name { get; set; }
        public List<CKTelcom> Telcom { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public List<CKExtention> Extention { get; set; }
        
    }
}
