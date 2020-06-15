using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class Muncity
    {
        public Muncity()
        {
            Facility = new HashSet<Facility>();
            Patient = new HashSet<Patient>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("province_id")]
        public int ProvinceId { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("Muncity")]
        public virtual ICollection<Facility> Facility { get; set; }
        [InverseProperty("Muncity")]
        public virtual ICollection<Patient> Patient { get; set; }
        [InverseProperty("Muncity")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
