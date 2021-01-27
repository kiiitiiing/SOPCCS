using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.SopViewModel
{
    public partial class AddSopModel
    {
        [Required]
        public int PatientId { get; set; }
        [Required]
        public string RequestedBy { get; set; }
        [Required]
        public string RequitionerContactNo { get; set; }
        [Required]
        public DateTime? DateOnsetSymptoms { get; set; }
        [Required]
        public string Swabber { get; set; }
        [Required]
        public DateTime? DTCollection { get; set; }
        [Required]
        public string SpecimenType { get; set; }
        [Required]
        public int NumSpec { get; set; }
        public bool Disabled { get; set; }
    }
}
