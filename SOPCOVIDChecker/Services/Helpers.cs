﻿using Microsoft.AspNetCore.Mvc;
using SOPCOVIDChecker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Services
{
    public static class Helpers
    {
        private static string FullName;
        public static string FixName(this string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static string Action(this ControllerContext context)
        {
            return context.RouteData.Values["action"].ToString();
        }

        public static List<KeyValuePair<string,string>> SpecimenTypes
        {
            get
            {
                return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("NPS","ETA "),
                    new KeyValuePair<string, string>("OPS","OPS"),
                    new KeyValuePair<string, string>("OPS & NPS","OPS & NPS"),
                    new KeyValuePair<string, string>("SERUM ","SERUM"),
                    new KeyValuePair<string, string>("ETA ","ETA "),
                    new KeyValuePair<string, string>("TA ","TA "),
                    new KeyValuePair<string, string>("SPUTUM ","SPUTUM "),
                    new KeyValuePair<string, string>("STOOL ","STOOL "),
                    new KeyValuePair<string, string>("BLOOD ","BLOOD "),
                    new KeyValuePair<string, string>("ENVIRONMENTAL SAMPLE","ENVIRONMENTAL SAMPLE"),
                    new KeyValuePair<string, string>("NGT ","NGT "),
                    new KeyValuePair<string, string>("ETA SWAB IN VTM/UTM","ETA SWAB IN VTM/UTM"),
                    new KeyValuePair<string, string>("OTHER","OTHER"),
                };
            }
        }

        public static List<KeyValuePair<string, string>> Filters
        {
            get
            {
                return new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("PROCESSING","Processing"),
                    new KeyValuePair<string, string>("RELEASED","Released")
                };
            }
        }

        public static string FirstToUpper(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Trim().ToLower();

                text = text.First().ToString().ToUpper() + text.Substring(1);

                return text;
            }
            else
                return "";
        }

        /*public static bool CheckPatient(this Patient patient)
        {
            if (patient.Province != null &&
                patient.Muncity != null &&
                patient.Barangay != null)
                return true;
            else
                return false;
        }*/


        public static ResultForm NewResultForm()
        {
            var form = new ResultForm
            {
                PerformedBy = null,
                VerifiedBy = null,
                ApprovedBy = null,
                CreatedBy = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return form;
        }

        public static string NameToUpper(this string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string[] names = name.Split(null);
                string fullname = "";
                foreach (var item in names)
                {
                    fullname += item.FirstToUpper() + " ";
                }
                return fullname;
            }
            else
            {
                return "";
            }
        }


        public static DateTime RemoveSeconds(this DateTime dateTime)
        {
            var dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            return dt;
        }

        public static string ComputeTimeFrame(this double minutes)
        {
            var min = Math.Floor(minutes);
            var minute = min == 0 ? "" : min + "m";
            var sec = Math.Round((minutes - min) * 60);
            var seconds = sec == 0 ? "" : sec + "s";
            var total = minute + " " + seconds;
            return total;
        }

        public static int ComputeAge(this DateTime dob)
        {
            var today = DateTime.Today;

            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public static int ComputeAge(this DateTime? dob)
        {
            var realDob = (DateTime)dob;
            var today = DateTime.Today;

            var age = today.Year - realDob.Year;

            if (realDob.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public static double ArchivedTime(DateTime date)
        {
            return Convert.ToInt32(DateTime.Now.Subtract(date).TotalMinutes);
        }

        public static Task<string> Result(this ResultForm result)
        {
            var status = "";
            if(result.CreatedBy == null)
            {
                status = "PROCESSING";
            }
            else if(result.CreatedBy != null && result.ApprovedBy == null)
            {
                status = "PROCESSING";
            }
            else
            {
                if (result.SopForm.PcrResult.Contains("Positive"))
                {
                    status = "RELEASED";
                }
                else
                {
                    status = "RELEASED";
                }
            }

            return Task.FromResult(status);
        }
        public static string GetDate(this DateTime date, string format)
        {
            if (date != default)
            {
                if (!format.Contains("tt"))
                {
                    return date.ToString(format);
                }
                else
                    return date.ToString(format, CultureInfo.InvariantCulture);
            }
            else
                return "";
        }

        public static string GetDate(this DateTime? date, string format)
        {
            var realDate = (DateTime)date;

            if (realDate != default)
            {
                if (!format.Contains("tt"))
                {
                    return realDate.ToString(format);
                }
                else
                    return realDate.ToString(format, CultureInfo.InvariantCulture);
            }
            else
                return "";
        }

        public static string CheckAddress(this string address)
        {
            return string.IsNullOrEmpty(address) ? "" : address + ", ";
        }

        public static string CheckString(this string text)
        {
            return string.IsNullOrEmpty(text) ? "" : text;
        }

        public static string AddressCheck(this int? id)
        {
            return id == null ? "" : id.ToString();
        }

        public static string GetSex(this int sex)
        {
            if (sex == 1)
                return "Male";
            else
                return "Female";
        }

        public static string GetAddress(this Facility facility)
        {
            string address = string.IsNullOrEmpty(facility.Address) ? "" : facility.Address + ", ";
            string barangay = facility.BarangayNavigation == null ? "" : facility.BarangayNavigation.Description + ", ";
            string muncity = facility.MuncityNavigation == null ? "" : facility.MuncityNavigation.Description + ", ";
            string province = facility.ProvinceNavigation == null ? "" : facility.ProvinceNavigation.Description;

            return address + barangay + muncity + province;
        }

        public static string GetAddress(Barangay barangay, Muncity muncity, Province province)
        {
            string barangays = barangay == null ? "" : barangay.Description + ", ";
            string muncitys = muncity == null ? "" : muncity.Description + ", ";
            string provinces = muncity == null ? "" : province.Description;

            return barangays + muncitys + provinces;
        }

        public static string GetAddress(this Patient patient)
        {
            string barangay = patient.CurrentBarangayNavigation == null ? "" : patient.CurrentBarangayNavigation.Description + ", ";
            string muncity = patient.CurrentMuncityNavigation == null ? "" : patient.CurrentMuncityNavigation.Description + ", ";
            string province = patient.CurrentProvinceNavigation == null ? "" : patient.CurrentProvinceNavigation.Description;
            string address = string.IsNullOrEmpty(patient.CurrentAddress) ? "" : patient.CurrentAddress + ", ";
            string purok = string.IsNullOrEmpty(patient.CurrentPurok) ? "" : patient.CurrentPurok + ", ";
            string sitio = string.IsNullOrEmpty(patient.CurrentSitio) ? "" : patient.CurrentSitio + ", ";

            return address + sitio + purok + barangay + muncity + province;
        }

        public static string GetPermanentAddress(this Patient patient)
        {
            string barangay = patient.PermanentBarangayNavigation == null ? "" : patient.PermanentBarangayNavigation.Description + ", ";
            string muncity = patient.PermanentMuncityNavigation == null ? "" : patient.PermanentMuncityNavigation.Description + ", ";
            string province = patient.PermanentProvinceNavigation == null ? "" : patient.PermanentProvinceNavigation.Description;
            string address = string.IsNullOrEmpty(patient.PermanentAddress) ? "" : patient.PermanentAddress + ", ";
            string purok = string.IsNullOrEmpty(patient.PermanentPurok) ? "" : patient.PermanentPurok + ", ";
            string sitio = string.IsNullOrEmpty(patient.PermanentSitio) ? "" : patient.PermanentSitio + ", ";

            return address + sitio + purok + barangay + muncity + province;
        }


        public static string GetAddress(this Sopusers patient)
        {
            string barangay = patient.BarangayNavigation == null ? "" : patient.BarangayNavigation.Description + ", ";
            string muncity = patient.MuncityNavigation == null ? "" : patient.MuncityNavigation.Description + ", ";
            string province = patient.ProvinceNavigation == null ? "" : patient.ProvinceNavigation.Description;

            return barangay + muncity + province;
        }

        public static string GetFullName(this Patient patient)
        {
            if (patient != null)
                return patient.Fname.CheckName() + " " + patient.Mname.CheckName() + " " + patient.Lname.CheckName();
            else
                return "";
        }

        public static string GetFullLastName(Sopusers user)
        {
            if (user != null)
                return user.Lname.CheckName() + ", " + user.Fname.CheckName() + " " + user.Mname.CheckName();
            else
                return "";
        }
        public static string GetFullLastName(this Patient patient)
        {
            if (patient != null)
                return patient.Lname.CheckName() + ", " + patient.Fname.CheckName() + " " + patient.Mname.CheckName();
            else
                return "";
        }
        public static string GetFullName(this Sopusers user)
        {
            if (user != null)
                return user.Fname.CheckName() + " " + user.Mname.CheckName() + " " + user.Lname.CheckName();
            else
                return "";  
        }

        public static string GetMDFullName(this Sopusers doctor)
        {
            if (doctor != null)
                FullName = "Dr. " + doctor.Fname.CheckName() + " " + doctor.Mname.CheckName() + " " + doctor.Lname.CheckName();
            else
                FullName = "";

            return FullName;
        }

        public static string CheckName(this string name)
        {
            return string.IsNullOrEmpty(name) ? "" : name;
        }
    }
}
