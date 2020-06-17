using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ResuViewModel
{
    public partial class SendToLab
    {
        [Required]
        public int ResultFormId { get; set; }
        [Required]
        public int LabAccId { get; set; }
    }
}
