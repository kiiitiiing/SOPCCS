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
        [Column("firstname")]
        [StringLength(255)]
        public string Firstname { get; set; }
        [Column("middlename")]
        [StringLength(255)]
        public string Middlename { get; set; }
        [Required]
        [Column("lastname")]
        [StringLength(255)]
        public string Lastname { get; set; }
        [Column("contact")]
        [StringLength(100)]
        public string Contact { get; set; }
        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [Column("user_level")]
        [StringLength(50)]
        public string UserLevel { get; set; }
        [Column("facility_id")]
        public int FacilityId { get; set; }
        [Column("barangay_id")]
        public int BarangayId { get; set; }
        [Column("muncity_id")]
        public int MuncityId { get; set; }
        [Column("province_id")]
        public int ProvinceId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(BarangayId))]
        [InverseProperty("Sopusers")]
        public virtual Barangay Barangay { get; set; }
        [ForeignKey(nameof(FacilityId))]
        [InverseProperty("Sopusers")]
        public virtual Facility Facility { get; set; }
        [ForeignKey(nameof(MuncityId))]
        [InverseProperty("Sopusers")]
        public virtual Muncity Muncity { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("Sopusers")]
        public virtual Province Province { get; set; }
    }
}
