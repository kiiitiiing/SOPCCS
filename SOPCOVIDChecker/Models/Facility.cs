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
            Sopform = new HashSet<Sopform>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column("abbrevation")]
        [StringLength(255)]
        public string Abbrevation { get; set; }
        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }
        [Column("barangay_id")]
        public int? BarangayId { get; set; }
        [Column("muncity_id")]
        public int? MuncityId { get; set; }
        [Column("province_id")]
        public int? ProvinceId { get; set; }
        [Column("contact")]
        [StringLength(255)]
        public string Contact { get; set; }
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column("picture")]
        [StringLength(255)]
        public string Picture { get; set; }
        [Column("chief_hospital")]
        [StringLength(100)]
        public string ChiefHospital { get; set; }
        [Column("hospital_level")]
        public int? HospitalLevel { get; set; }
        [Column("hospital_type")]
        [StringLength(20)]
        public string HospitalType { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(BarangayId))]
        [InverseProperty("Facility")]
        public virtual Barangay Barangay { get; set; }
        [ForeignKey(nameof(MuncityId))]
        [InverseProperty("Facility")]
        public virtual Muncity Muncity { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("Facility")]
        public virtual Province Province { get; set; }
        [InverseProperty("DiseaseReportingUnit")]
        public virtual ICollection<Sopform> Sopform { get; set; }
        [InverseProperty("Facility")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
