using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ResultViewModel
{
    public partial class ResultFormModel
    {
        public int Id { get; set; }
        public int SopFormId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public DateTime DoB { get; set; }
        public string Location { get; set; }
        public string Requisitioner { get; set; }
        public string SpecimenType { get; set; }
        public string Address { get; set; }
        public DateTime DTSpecimeCollection { get; set; }
        [Required]
        public string SampleID { get; set; }
        public DateTime AdmissionDate { get; set; }
        [Required]
        public DateTime DTSpecimenReceipt { get; set; }
        [Required]
        public DateTime DTReleaseResult { get; set; }
        [Required]
        public string TestResult { get; set; }
        public string Comments { get; set; }
        [Required]
        public int? Performed { get; set; }
        [Required]
        public int? Verified { get; set; }
        [Required]
        public int? Approved { get; set; }
    }
}
