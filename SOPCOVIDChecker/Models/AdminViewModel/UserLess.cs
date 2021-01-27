using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.AdminViewModel
{
    public partial class UserLess
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string Facility { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Fullname
        {
            get
            {
                return $"{Fname} {(string.IsNullOrEmpty(Mname) ? "" : Mname + " ")}{Lname}";
            }
        }
    }
}
