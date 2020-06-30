using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SOPCOVIDChecker.Models.SopViewModel
{
    public partial class AddPatientModel
    {
        [Required]
        [Column("fname")]
        [StringLength(70)]
        public string Fname { get; set; }
        [Column("mname")]
        [StringLength(70)]
        public string Mname { get; set; }
        [Required]
        [Column("lname")]
        [StringLength(70)]
        public string Lname { get; set; }
        [Column("dob", TypeName = "date")]
        public DateTime Dob { get; set; }
        [Required]
        [Column("sex")]
        [StringLength(255)]
        public string Sex { get; set; }
        [Required]
        [Column("contact_no")]
        [StringLength(50)]
        public string ContactNo { get; set; }
        [Column("current_barangay")]
        public int CurrentBarangay { get; set; }
        [Column("current_muncity")]
        public int CurrentMuncity { get; set; }
        [Column("current_province")]
        public int CurrentProvince { get; set; }
        [Column("current_purok")]
        [StringLength(100)]
        public string CurrentPurok { get; set; }
        [Column("current_sitio")]
        [StringLength(100)]
        public string CurrentSitio { get; set; }
        [Required]
        [Column("current_address")]
        [StringLength(255)]
        public string CurrentAddress { get; set; }
        [Required]
        [Column("permanent_baragnay")]
        public int? PermanentBaragnay { get; set; }
        [Required]
        [Column("permanent_muncity")]
        public int? PermanentMuncity { get; set; }
        [Column("permanent_province")]
        public int? PermanentProvince { get; set; }
        [Column("permanent_purok")]
        [StringLength(100)]
        public string PermanentPurok { get; set; }
        [Column("permanent_sitio")]
        [StringLength(100)]
        public string PermanentSitio { get; set; }
        [Required]
        [Column("permanent_address")]
        [StringLength(255)]
        public string PermanentAddress { get; set; }
        public bool Disabled { get; set; }
    }
}
