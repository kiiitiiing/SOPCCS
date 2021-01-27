using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ResultViewModel
{
    public partial class ArrivedModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime? Arrived { get; set; }
    }
}
