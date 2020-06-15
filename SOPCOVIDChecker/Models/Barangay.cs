using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class Barangay
    {
        public Barangay()
        {
            Facility = new HashSet<Facility>();
            Patient = new HashSet<Patient>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("province_id")]
        public int? ProvinceId { get; set; }
        [Column("muncity_id")]
        public int MuncityId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("old_target")]
        public int OldTarget { get; set; }
        [Column("target")]
        public int Target { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("Barangay")]
        public virtual ICollection<Facility> Facility { get; set; }
        [InverseProperty("Barangay")]
        public virtual ICollection<Patient> Patient { get; set; }
        [InverseProperty("Barangay")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
