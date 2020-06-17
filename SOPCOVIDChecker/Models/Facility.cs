using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class Facility
    {
        public Facility()
        {
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("facility_code")]
        [StringLength(100)]
        public string FacilityCode { get; set; }
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column("abbr")]
        [StringLength(255)]
        public string Abbr { get; set; }
        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }
        [Column("barangay")]
        public int? Barangay { get; set; }
        [Column("muncity")]
        public int? Muncity { get; set; }
        [Column("province")]
        public int Province { get; set; }
        [Column("contact_no")]
        [StringLength(255)]
        public string ContactNo { get; set; }
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("picture")]
        [StringLength(255)]
        public string Picture { get; set; }
        [Column("chief_hospital")]
        [StringLength(255)]
        public string ChiefHospital { get; set; }
        [Column("level")]
        [StringLength(50)]
        public string Level { get; set; }
        [Column("hospital_type")]
        [StringLength(50)]
        public string HospitalType { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(Barangay))]
        [InverseProperty("Facility")]
        public virtual Barangay BarangayNavigation { get; set; }
        [ForeignKey(nameof(Muncity))]
        [InverseProperty("Facility")]
        public virtual Muncity MuncityNavigation { get; set; }
        [ForeignKey(nameof(Province))]
        [InverseProperty("Facility")]
        public virtual Province ProvinceNavigation { get; set; }
        [InverseProperty("Facility")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
