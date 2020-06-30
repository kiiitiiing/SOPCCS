using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class Province
    {
        public Province()
        {
            Facility = new HashSet<Facility>();
            PatientCurrentProvinceNavigation = new HashSet<Patient>();
            PatientPermanentProvinceNavigation = new HashSet<Patient>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("ProvinceNavigation")]
        public virtual ICollection<Facility> Facility { get; set; }
        [InverseProperty(nameof(Patient.CurrentProvinceNavigation))]
        public virtual ICollection<Patient> PatientCurrentProvinceNavigation { get; set; }
        [InverseProperty(nameof(Patient.PermanentProvinceNavigation))]
        public virtual ICollection<Patient> PatientPermanentProvinceNavigation { get; set; }
        [InverseProperty("ProvinceNavigation")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
