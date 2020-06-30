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
            PatientCurrentMuncityNavigation = new HashSet<Patient>();
            PatientPermanentMuncityNavigation = new HashSet<Patient>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("province")]
        public int Province { get; set; }
        [Required]
        [Column("description")]
        [StringLength(50)]
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [InverseProperty("MuncityNavigation")]
        public virtual ICollection<Facility> Facility { get; set; }
        [InverseProperty(nameof(Patient.CurrentMuncityNavigation))]
        public virtual ICollection<Patient> PatientCurrentMuncityNavigation { get; set; }
        [InverseProperty(nameof(Patient.PermanentMuncityNavigation))]
        public virtual ICollection<Patient> PatientPermanentMuncityNavigation { get; set; }
        [InverseProperty("MuncityNavigation")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
