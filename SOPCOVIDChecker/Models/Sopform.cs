﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPCOVIDChecker.Models
{
    [Table("SOPForm")]
    public partial class Sopform
    {
        public Sopform()
        {
            ResultForm = new HashSet<ResultForm>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("sample_id")]
        [StringLength(50)]
        public string SampleId { get; set; }
        [Column("pcr_result")]
        [StringLength(255)]
        public string PcrResult { get; set; }
        [Column("disease_reporting_unit_id")]
        public int DiseaseReportingUnitId { get; set; }
        [Column("datetime_collection")]
        public DateTime DatetimeCollection { get; set; }
        [Required]
        [Column("requested_by")]
        [StringLength(100)]
        public string RequestedBy { get; set; }
        [Column("requester_contact")]
        [StringLength(50)]
        public string RequesterContact { get; set; }
        [Required]
        [Column("type_speciment")]
        [StringLength(50)]
        public string TypeSpeciment { get; set; }
        [Column("datetime_speciment_receipt")]
        public DateTime DatetimeSpecimentReceipt { get; set; }
        [Column("date_result", TypeName = "date")]
        public DateTime DateResult { get; set; }
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(DiseaseReportingUnitId))]
        [InverseProperty(nameof(Facility.Sopform))]
        public virtual Facility DiseaseReportingUnit { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("Sopform")]
        public virtual Patient Patient { get; set; }
        [InverseProperty("SopForm")]
        public virtual ICollection<ResultForm> ResultForm { get; set; }
    }
}
