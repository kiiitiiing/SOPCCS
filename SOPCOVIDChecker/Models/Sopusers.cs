using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    [Table("SOPUsers")]
    public partial class Sopusers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("username")]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }
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
        [Column("contact_no")]
        [StringLength(50)]
        public string ContactNo { get; set; }
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [Column("user_level")]
        [StringLength(50)]
        public string UserLevel { get; set; }
        [Column("facility_id")]
        public int FacilityId { get; set; }
        [Column("barangay")]
        public int? Barangay { get; set; }
        [Column("muncity")]
        public int? Muncity { get; set; }
        [Column("province")]
        public int Province { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(Barangay))]
        [InverseProperty("Sopusers")]
        public virtual Barangay BarangayNavigation { get; set; }
        [ForeignKey(nameof(FacilityId))]
        [InverseProperty("Sopusers")]
        public virtual Facility Facility { get; set; }
        [ForeignKey(nameof(Muncity))]
        [InverseProperty("Sopusers")]
        public virtual Muncity MuncityNavigation { get; set; }
        [ForeignKey(nameof(Province))]
        [InverseProperty("Sopusers")]
        public virtual Province ProvinceNavigation { get; set; }
    }
}
