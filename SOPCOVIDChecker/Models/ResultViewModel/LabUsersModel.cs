using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ResultViewModel
{
    public partial class LabUsersModel
    {
        [Required]
        [Display(Name = "first name")]
        public string Firstname { get; set; }
        [Display(Name = "middle name")]
        public string Middlename { get; set; }
        [Required]
        [Display(Name = "last name")]
        public string Lastname { get; set; }
        [Required]
        [Display(Name = "facility")]
        public int FacilityId { get; set; }
        [Required]
        [Display(Name = "designation")]
        public string Designation { get; set; }
        [Required]
        [Display(Name = "license number")]
        public string LicenseNo { get; set; }
        [Display(Name = "contact number")]
        public string ContactNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "role")]
        public string Role { get; set; }
        [Display(Name = "titles")]
        public string Postfix { get; set; }

    }
}
