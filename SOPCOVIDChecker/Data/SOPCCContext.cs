using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SOPCOVIDChecker.Models;

namespace SOPCOVIDChecker.Data
{
    public partial class SOPCCContext : DbContext
    {
        public SOPCCContext()
        {
        }

        public SOPCCContext(DbContextOptions<SOPCCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Barangay> Barangay { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<Muncity> Muncity { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<ResultForm> ResultForm { get; set; }
        public virtual DbSet<Sopform> Sopform { get; set; }
        public virtual DbSet<Sopusers> Sopusers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=ROCKY\\SQLEXPRESS;Initial Catalog=SOPCOVIDDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Barangay>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Abbr).IsUnicode(false);

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.ChiefHospital).IsUnicode(false);

                entity.Property(e => e.ContactNo).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.FacilityCode).IsUnicode(false);

                entity.Property(e => e.HospitalType).IsUnicode(false);

                entity.Property(e => e.Level).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Picture).IsUnicode(false);

                entity.HasOne(d => d.BarangayNavigation)
                    .WithMany(p => p.Facility)
                    .HasForeignKey(d => d.Barangay)
                    .HasConstraintName("FK_Facility_Barangay");

                entity.HasOne(d => d.MuncityNavigation)
                    .WithMany(p => p.Facility)
                    .HasForeignKey(d => d.Muncity)
                    .HasConstraintName("FK_Facility_Muncity");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.Facility)
                    .HasForeignKey(d => d.Province)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Facility_Province");
            });

            modelBuilder.Entity<Muncity>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.ContactNo).IsUnicode(false);

                entity.Property(e => e.Fname).IsUnicode(false);

                entity.Property(e => e.Lname).IsUnicode(false);

                entity.Property(e => e.Mname).IsUnicode(false);

                entity.Property(e => e.Sex)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.BarangayNavigation)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.Barangay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Barangay");

                entity.HasOne(d => d.MuncityNavigation)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.Muncity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Muncity");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.Province)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patient_Province");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<ResultForm>(entity =>
            {
                entity.Property(e => e.BiologicalReferrence).IsUnicode(false);

                entity.Property(e => e.FinalResult).IsUnicode(false);

                entity.Property(e => e.Interpretation).IsUnicode(false);

                entity.Property(e => e.LabTestPerformed).IsUnicode(false);

                entity.Property(e => e.TestResult).IsUnicode(false);

                entity.Property(e => e.TestResultsUnits).IsUnicode(false);

                entity.HasOne(d => d.ApprovedByNavigation)
                    .WithMany(p => p.ResultFormApprovedByNavigation)
                    .HasForeignKey(d => d.ApprovedBy)
                    .HasConstraintName("FK_Approve_SOPUsers");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ResultFormCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ResultForm_SOPUsers");

                entity.HasOne(d => d.PerformedByNavigation)
                    .WithMany(p => p.ResultFormPerformedByNavigation)
                    .HasForeignKey(d => d.PerformedBy)
                    .HasConstraintName("FK_Perform_SOPUsers");

                entity.HasOne(d => d.SopForm)
                    .WithMany(p => p.ResultForm)
                    .HasForeignKey(d => d.SopFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResultForm_SOPForm");

                entity.HasOne(d => d.VerifiedByNavigation)
                    .WithMany(p => p.ResultFormVerifiedByNavigation)
                    .HasForeignKey(d => d.VerifiedBy)
                    .HasConstraintName("FK_Verify_SOPUsers");
            });

            modelBuilder.Entity<Sopform>(entity =>
            {
                entity.Property(e => e.PcrResult).IsUnicode(false);

                entity.Property(e => e.RequestedBy).IsUnicode(false);

                entity.Property(e => e.RequesterContact).IsUnicode(false);

                entity.Property(e => e.SampleId).IsUnicode(false);

                entity.Property(e => e.TypeSpecimen).IsUnicode(false);

                entity.HasOne(d => d.DiseaseReportingUnit)
                    .WithMany(p => p.Sopform)
                    .HasForeignKey(d => d.DiseaseReportingUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOPForm_SOPUsers");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Sopform)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOPForm_Patient");
            });

            modelBuilder.Entity<Sopusers>(entity =>
            {
                entity.Property(e => e.ContactNo).IsUnicode(false);

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Fname).IsUnicode(false);

                entity.Property(e => e.LicenseNo).IsUnicode(false);

                entity.Property(e => e.Lname).IsUnicode(false);

                entity.Property(e => e.Mname).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Postfix).IsUnicode(false);

                entity.Property(e => e.UserLevel).IsUnicode(false);

                entity.Property(e => e.Username).IsUnicode(false);

                entity.HasOne(d => d.BarangayNavigation)
                    .WithMany(p => p.Sopusers)
                    .HasForeignKey(d => d.Barangay)
                    .HasConstraintName("FK_SOPUsers_Barangay");

                entity.HasOne(d => d.Facility)
                    .WithMany(p => p.Sopusers)
                    .HasForeignKey(d => d.FacilityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOPUsers_Facility");

                entity.HasOne(d => d.MuncityNavigation)
                    .WithMany(p => p.Sopusers)
                    .HasForeignKey(d => d.Muncity)
                    .HasConstraintName("FK_SOPUsers_Muncity");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.Sopusers)
                    .HasForeignKey(d => d.Province)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SOPUsers_Province");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
