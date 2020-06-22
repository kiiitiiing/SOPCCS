using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Services
{
    public partial class ListContainer
    {
        public static List<KeyValuePair<int, string>> HospitalLevel
        {
            get
            {
                return new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>(1, "Level 1"),
                    new KeyValuePair<int, string>(2, "Level 2"),
                    new KeyValuePair<int, string>(3, "Level 3")
                };
            }
        }

        public static List<KeyValuePair<string, string>> HospitalType
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("government", "Government"),
                    new KeyValuePair<string, string>("private", "Private")
                };
            }
        }
    }
}
