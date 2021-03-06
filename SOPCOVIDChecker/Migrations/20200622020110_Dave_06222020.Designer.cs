﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SOPCOVIDChecker.Data;

namespace SOPCOVIDChecker.Migrations
{
    [DbContext(typeof(SOPCCContext))]
    [Migration("20200622020110_Dave_06222020")]
    partial class Dave_06222020
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SOPCOVIDChecker.Models.Barangay", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("Muncity")
                        .HasColumnName("muncity")
                        .HasColumnType("int");

                    b.Property<int>("OldTarget")
                        .HasColumnName("old_target")
                        .HasColumnType("int");

                    b.Property<int?>("Province")
                        .HasColumnName("province")
                        .HasColumnType("int");

                    b.Property<int>("Target")
                        .HasColumnName("target")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Barangay");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Facility", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id")
                        .HasColumnType("int");

                    b.Property<string>("Abbr")
                        .HasColumnName("abbr")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Address")
                        .HasColumnName("address")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int?>("Barangay")
                        .HasColumnName("barangay")
                        .HasColumnType("int");

                    b.Property<string>("ChiefHospital")
                        .HasColumnName("chief_hospital")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("ContactNo")
                        .HasColumnName("contact_no")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("FacilityCode")
                        .HasColumnName("facility_code")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("HospitalType")
                        .HasColumnName("hospital_type")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Level")
                        .HasColumnName("level")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("Muncity")
                        .HasColumnName("muncity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Picture")
                        .HasColumnName("picture")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("Province")
                        .HasColumnName("province")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Barangay");

                    b.HasIndex("Muncity");

                    b.HasIndex("Province");

                    b.ToTable("Facility");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Muncity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("Province")
                        .HasColumnName("province")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Muncity");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnName("address")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("Barangay")
                        .HasColumnName("barangay")
                        .HasColumnType("int");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnName("contact_no")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Dob")
                        .HasColumnName("dob")
                        .HasColumnType("date");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasColumnName("fname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasColumnName("lname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<string>("Mname")
                        .HasColumnName("mname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<int>("Muncity")
                        .HasColumnName("muncity")
                        .HasColumnType("int");

                    b.Property<int>("Province")
                        .HasColumnName("province")
                        .HasColumnType("int");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnName("sex")
                        .HasColumnType("char(255)")
                        .IsFixedLength(true)
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Barangay");

                    b.HasIndex("Muncity");

                    b.HasIndex("Province");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Province");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.ResultForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApprovedBy")
                        .HasColumnName("approved_by")
                        .HasColumnType("int");

                    b.Property<string>("BiologicalReferrence")
                        .HasColumnName("biological_referrence")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Comments")
                        .HasColumnName("comments")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreatedBy")
                        .HasColumnName("created_by")
                        .HasColumnType("int");

                    b.Property<string>("FinalResult")
                        .IsRequired()
                        .HasColumnName("final_result")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Interpretation")
                        .IsRequired()
                        .HasColumnName("interpretation")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("LabTestPerformed")
                        .IsRequired()
                        .HasColumnName("lab_test_performed")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int?>("PerformedBy")
                        .HasColumnName("performed_by")
                        .HasColumnType("int");

                    b.Property<int>("SopFormId")
                        .HasColumnName("sop_form_id")
                        .HasColumnType("int");

                    b.Property<string>("TestResult")
                        .IsRequired()
                        .HasColumnName("test_result")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("TestResultsUnits")
                        .HasColumnName("test_results_units")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<int?>("VerifiedBy")
                        .HasColumnName("verified_by")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedBy");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("PerformedBy");

                    b.HasIndex("SopFormId");

                    b.HasIndex("VerifiedBy");

                    b.ToTable("ResultForm");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Sopform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOnsetSymptoms")
                        .HasColumnName("date_onset_symptoms")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateResult")
                        .HasColumnName("date_result")
                        .HasColumnType("date");

                    b.Property<DateTime>("DatetimeCollection")
                        .HasColumnName("datetime_collection")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatetimeSpecimenReceipt")
                        .HasColumnName("datetime_specimen_receipt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DiseaseReportingUnitId")
                        .HasColumnName("disease_reporting_unit_id")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnName("patient_id")
                        .HasColumnType("int");

                    b.Property<string>("PcrResult")
                        .IsRequired()
                        .HasColumnName("pcr_result")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("RequestedBy")
                        .IsRequired()
                        .HasColumnName("requested_by")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequesterContact")
                        .IsRequired()
                        .HasColumnName("requester_contact")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("SampleId")
                        .IsRequired()
                        .HasColumnName("sample_id")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Swabber")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("TypeSpecimen")
                        .IsRequired()
                        .HasColumnName("type_specimen")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DiseaseReportingUnitId");

                    b.HasIndex("PatientId");

                    b.ToTable("SOPForm");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Sopusers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Barangay")
                        .HasColumnName("barangay")
                        .HasColumnType("int");

                    b.Property<string>("ContactNo")
                        .HasColumnName("contact_no")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .HasColumnName("designation")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<int>("FacilityId")
                        .HasColumnName("facility_id")
                        .HasColumnType("int");

                    b.Property<string>("Fname")
                        .IsRequired()
                        .HasColumnName("fname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<string>("LicenseNo")
                        .HasColumnName("license_no")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Lname")
                        .IsRequired()
                        .HasColumnName("lname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<string>("Mname")
                        .HasColumnName("mname")
                        .HasColumnType("varchar(70)")
                        .HasMaxLength(70)
                        .IsUnicode(false);

                    b.Property<int?>("Muncity")
                        .HasColumnName("muncity")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Postfix")
                        .HasColumnName("postfix")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("Province")
                        .HasColumnName("province")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserLevel")
                        .IsRequired()
                        .HasColumnName("user_level")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("username")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Barangay");

                    b.HasIndex("FacilityId");

                    b.HasIndex("Muncity");

                    b.HasIndex("Province");

                    b.ToTable("SOPUsers");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Facility", b =>
                {
                    b.HasOne("SOPCOVIDChecker.Models.Barangay", "BarangayNavigation")
                        .WithMany("Facility")
                        .HasForeignKey("Barangay")
                        .HasConstraintName("FK_Facility_Barangay");

                    b.HasOne("SOPCOVIDChecker.Models.Muncity", "MuncityNavigation")
                        .WithMany("Facility")
                        .HasForeignKey("Muncity")
                        .HasConstraintName("FK_Facility_Muncity");

                    b.HasOne("SOPCOVIDChecker.Models.Province", "ProvinceNavigation")
                        .WithMany("Facility")
                        .HasForeignKey("Province")
                        .HasConstraintName("FK_Facility_Province")
                        .IsRequired();
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Patient", b =>
                {
                    b.HasOne("SOPCOVIDChecker.Models.Barangay", "BarangayNavigation")
                        .WithMany("Patient")
                        .HasForeignKey("Barangay")
                        .HasConstraintName("FK_Patient_Barangay")
                        .IsRequired();

                    b.HasOne("SOPCOVIDChecker.Models.Muncity", "MuncityNavigation")
                        .WithMany("Patient")
                        .HasForeignKey("Muncity")
                        .HasConstraintName("FK_Patient_Muncity")
                        .IsRequired();

                    b.HasOne("SOPCOVIDChecker.Models.Province", "ProvinceNavigation")
                        .WithMany("Patient")
                        .HasForeignKey("Province")
                        .HasConstraintName("FK_Patient_Province")
                        .IsRequired();
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.ResultForm", b =>
                {
                    b.HasOne("SOPCOVIDChecker.Models.Sopusers", "ApprovedByNavigation")
                        .WithMany("ResultFormApprovedByNavigation")
                        .HasForeignKey("ApprovedBy")
                        .HasConstraintName("FK_Approve_SOPUsers");

                    b.HasOne("SOPCOVIDChecker.Models.Sopusers", "CreatedByNavigation")
                        .WithMany("ResultFormCreatedByNavigation")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("FK_ResultForm_SOPUsers");

                    b.HasOne("SOPCOVIDChecker.Models.Sopusers", "PerformedByNavigation")
                        .WithMany("ResultFormPerformedByNavigation")
                        .HasForeignKey("PerformedBy")
                        .HasConstraintName("FK_Perform_SOPUsers");

                    b.HasOne("SOPCOVIDChecker.Models.Sopform", "SopForm")
                        .WithMany("ResultForm")
                        .HasForeignKey("SopFormId")
                        .HasConstraintName("FK_ResultForm_SOPForm")
                        .IsRequired();

                    b.HasOne("SOPCOVIDChecker.Models.Sopusers", "VerifiedByNavigation")
                        .WithMany("ResultFormVerifiedByNavigation")
                        .HasForeignKey("VerifiedBy")
                        .HasConstraintName("FK_Verify_SOPUsers");
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Sopform", b =>
                {
                    b.HasOne("SOPCOVIDChecker.Models.Sopusers", "DiseaseReportingUnit")
                        .WithMany("Sopform")
                        .HasForeignKey("DiseaseReportingUnitId")
                        .HasConstraintName("FK_SOPForm_SOPUsers")
                        .IsRequired();

                    b.HasOne("SOPCOVIDChecker.Models.Patient", "Patient")
                        .WithMany("Sopform")
                        .HasForeignKey("PatientId")
                        .HasConstraintName("FK_SOPForm_Patient")
                        .IsRequired();
                });

            modelBuilder.Entity("SOPCOVIDChecker.Models.Sopusers", b =>
                {
                    b.HasOne("SOPCOVIDChecker.Models.Barangay", "BarangayNavigation")
                        .WithMany("Sopusers")
                        .HasForeignKey("Barangay")
                        .HasConstraintName("FK_SOPUsers_Barangay");

                    b.HasOne("SOPCOVIDChecker.Models.Facility", "Facility")
                        .WithMany("Sopusers")
                        .HasForeignKey("FacilityId")
                        .HasConstraintName("FK_SOPUsers_Facility")
                        .IsRequired();

                    b.HasOne("SOPCOVIDChecker.Models.Muncity", "MuncityNavigation")
                        .WithMany("Sopusers")
                        .HasForeignKey("Muncity")
                        .HasConstraintName("FK_SOPUsers_Muncity");

                    b.HasOne("SOPCOVIDChecker.Models.Province", "ProvinceNavigation")
                        .WithMany("Sopusers")
                        .HasForeignKey("Province")
                        .HasConstraintName("FK_SOPUsers_Province")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
