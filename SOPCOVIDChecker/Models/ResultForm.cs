using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    public partial class ResultForm
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("lab_test_performed")]
        [StringLength(255)]
        public string LabTestPerformed { get; set; }
        [Required]
        [Column("test_result")]
        [StringLength(255)]
        public string TestResult { get; set; }
        [Column("test_results_units")]
        [StringLength(255)]
        public string TestResultsUnits { get; set; }
        [Column("biological_referrence")]
        [StringLength(255)]
        public string BiologicalReferrence { get; set; }
        [Required]
        [Column("final_result")]
        [StringLength(255)]
        public string FinalResult { get; set; }
        [Required]
        [Column("interpretation")]
        [StringLength(255)]
        public string Interpretation { get; set; }
        [Column("comments", TypeName = "text")]
        public string Comments { get; set; }
        [Required]
        [Column("performed_by")]
        [StringLength(255)]
        public string PerformedBy { get; set; }
        [Required]
        [Column("verified_by")]
        [StringLength(255)]
        public string VerifiedBy { get; set; }
        [Required]
        [Column("approved_by")]
        [StringLength(255)]
        public string ApprovedBy { get; set; }
        [Column("sop_form_id")]
        public int SopFormId { get; set; }
        [Column("created_by")]
        public int? CreatedBy { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(Sopusers.ResultForm))]
        public virtual Sopusers CreatedByNavigation { get; set; }
        [ForeignKey(nameof(SopFormId))]
        [InverseProperty(nameof(Sopform.ResultForm))]
        public virtual Sopform SopForm { get; set; }
    }
}
