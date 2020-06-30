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
            PatientCurrentBarangayNavigation = new HashSet<Patient>();
            PatientPermanentBarangayNavigation = new HashSet<Patient>();
            Sopusers = new HashSet<Sopusers>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("province")]
        public int? Province { get; set; }
        [Column("muncity")]
        public int Muncity { get; set; }
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

        [InverseProperty("BarangayNavigation")]
        public virtual ICollection<Facility> Facility { get; set; }
        [InverseProperty(nameof(Patient.CurrentBarangayNavigation))]
        public virtual ICollection<Patient> PatientCurrentBarangayNavigation { get; set; }
        [InverseProperty(nameof(Patient.PermanentBarangayNavigation))]
        public virtual ICollection<Patient> PatientPermanentBarangayNavigation { get; set; }
        [InverseProperty("BarangayNavigation")]
        public virtual ICollection<Sopusers> Sopusers { get; set; }
    }
}
