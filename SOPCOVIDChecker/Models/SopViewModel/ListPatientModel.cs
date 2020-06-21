using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.SopViewModel
{
    public partial class ListPatientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string DateOfBirth { get; set; }
        public int BarangayId { get; set; }
        public int MuncutyId { get; set; }
        public int ProvinceId { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }
}
