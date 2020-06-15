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
        [Column("firstname")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Column("middlename")]
        [StringLength(50)]
        public string Middlename { get; set; }
        [Required]
        [Column("lastname")]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Column("date_of_birth", TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Column("age")]
        public int Age { get; set; }
        [Column("gender")]
        public bool Gender { get; set; }
        [Column("contact_number")]
        [StringLength(255)]
        public string ContactNumber { get; set; }
        [Column("barangay_id")]
        public int BarangayId { get; set; }
        [Column("muncity_id")]
        public int MuncityId { get; set; }
        [Column("province_id")]
        public int ProvinceId { get; set; }
        [Column("address")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(BarangayId))]
        [InverseProperty("Patient")]
        public virtual Barangay Barangay { get; set; }
        [ForeignKey(nameof(MuncityId))]
        [InverseProperty("Patient")]
        public virtual Muncity Muncity { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("Patient")]
        public virtual Province Province { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<Sopform> Sopform { get; set; }
    }
}
