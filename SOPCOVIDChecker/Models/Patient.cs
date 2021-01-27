using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Sopform = new HashSet<Sopform>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
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
        [StringLength(100)]
        public string ContactNo { get; set; }
        [Required]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }
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
        [Column("permanent_barangay")]
        public int PermanentBarangay { get; set; }
        [Column("permanent_muncity")]
        public int PermanentMuncity { get; set; }
        [Column("permanent_province")]
        public int PermanentProvince { get; set; }
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
        [Required]
        [StringLength(50)]
        public string PhicMembershipType { get; set; }
        [Required]
        [Column("PIN")]
        [StringLength(50)]
        public string Pin { get; set; }
        public bool Employed { get; set; }
        [Column("PEN")]
        [StringLength(50)]
        public string Pen { get; set; }
        [Required]
        [StringLength(100)]
        public string EmployerName { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(CurrentBarangay))]
        [InverseProperty(nameof(Barangay.PatientCurrentBarangayNavigation))]
        public virtual Barangay CurrentBarangayNavigation { get; set; }
        [ForeignKey(nameof(CurrentMuncity))]
        [InverseProperty(nameof(Muncity.PatientCurrentMuncityNavigation))]
        public virtual Muncity CurrentMuncityNavigation { get; set; }
        [ForeignKey(nameof(CurrentProvince))]
        [InverseProperty(nameof(Province.PatientCurrentProvinceNavigation))]
        public virtual Province CurrentProvinceNavigation { get; set; }
        [ForeignKey(nameof(PermanentBarangay))]
        [InverseProperty(nameof(Barangay.PatientPermanentBarangayNavigation))]
        public virtual Barangay PermanentBarangayNavigation { get; set; }
        [ForeignKey(nameof(PermanentMuncity))]
        [InverseProperty(nameof(Muncity.PatientPermanentMuncityNavigation))]
        public virtual Muncity PermanentMuncityNavigation { get; set; }
        [ForeignKey(nameof(PermanentProvince))]
        [InverseProperty(nameof(Province.PatientPermanentProvinceNavigation))]
        public virtual Province PermanentProvinceNavigation { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<Sopform> Sopform { get; set; }
    }
}
