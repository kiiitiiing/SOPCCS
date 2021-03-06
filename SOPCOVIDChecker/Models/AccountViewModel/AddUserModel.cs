﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.AccountViewModels
{
    public partial class AddUserModel
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
        [Display(Name = "contact number")]
        public string ContactNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "username")]
        public string Username { get; set; }
        [Required]
        public int Province { get; set; }
        [Required]
        public int? Muncity { get; set; }
        [Required]
        public int? Barangay { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "confirm password")]
        public string ConfirmPassword { get; set; }
        public string Level { get; set; }

    }
}
