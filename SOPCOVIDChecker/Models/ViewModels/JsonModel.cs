using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.ViewModels
{
    public partial class JsonModel
    {
        public bool IsValid { get; set; }
        public string Html { get; set; }
        public string Toast { get; set; }
        public string Type 
        {
            get
            {
                if (IsValid)
                    return "success";
                else
                    return "error";
            }
        }
    }
}
