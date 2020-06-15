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
        [Column("contact_no")]
        [StringLength(50)]
        public string ContactNo { get; set; }
        [Column("barangay")]
        public int Barangay { get; set; }
        [Column("muncity")]
        public int Muncity { get; set; }
        [Column("province")]
        public int Province { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(Barangay))]
        [InverseProperty("Patient")]
        public virtual Barangay BarangayNavigation { get; set; }
        [ForeignKey(nameof(Muncity))]
        [InverseProperty("Patient")]
        public virtual Muncity MuncityNavigation { get; set; }
        [ForeignKey(nameof(Province))]
        [InverseProperty("Patient")]
        public virtual Province ProvinceNavigation { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<Sopform> Sopform { get; set; }
    }
}
