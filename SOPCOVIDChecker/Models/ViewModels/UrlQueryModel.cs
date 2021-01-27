using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ViewModels
{
    public partial class UrlQueryModel
    {
        public string Search { get; set; }
        public string DateRange { get; set; }
        public string Filter { get; set; }
        public int? Page { get; set; }
    }
}
