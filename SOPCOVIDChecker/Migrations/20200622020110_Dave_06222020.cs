using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SOPCOVIDChecker.Migrations
{
    public partial class Dave_06222020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barangay",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    province = table.Column<int>(nullable: true),
                    muncity = table.Column<int>(nullable: false),
                    description = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    old_target = table.Column<int>(nullable: false),
                    target = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barangay", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Muncity",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    province = table.Column<int>(nullable: false),
                    description = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muncity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    facility_code = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    name = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    abbr = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    address = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    barangay = table.Column<int>(nullable: true),
                    muncity = table.Column<int>(nullable: true),
                    province = table.Column<int>(nullable: false),
                    contact_no = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    status = table.Column<int>(nullable: false),
                    picture = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    chief_hospital = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    level = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    hospital_type = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.id);
                    table.ForeignKey(
                        name: "FK_Facility_Barangay",
                        column: x => x.barangay,
                        principalTable: "Barangay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facility_Muncity",
                        column: x => x.muncity,
                        principalTable: "Muncity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Facility_Province",
                        column: x => x.province,
                        principalTable: "Province",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fname = table.Column<string>(unicode: false, maxLength: 70, nullable: false),
                    mname = table.Column<string>(unicode: false, maxLength: 70, nullable: true),
                    lname = table.Column<string>(unicode: false, maxLength: 70, nullable: false),
                    dob = table.Column<DateTime>(type: "date", nullable: false),
                    sex = table.Column<string>(unicode: false, fixedLength: true, maxLength: 255, nullable: false),
                    contact_no = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    barangay = table.Column<int>(nullable: false),
                    muncity = table.Column<int>(nullable: false),
                    province = table.Column<int>(nullable: false),
                    address = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.id);
                    table.ForeignKey(
                        name: "FK_Patient_Barangay",
                        column: x => x.barangay,
                        principalTable: "Barangay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Muncity",
                        column: x => x.muncity,
                        principalTable: "Muncity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Province",
                        column: x => x.province,
                        principalTable: "Province",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SOPUsers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    password = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    fname = table.Column<string>(unicode: false, maxLength: 70, nullable: false),
                    mname = table.Column<string>(unicode: false, maxLength: 70, nullable: true),
                    lname = table.Column<string>(unicode: false, maxLength: 70, nullable: false),
                    contact_no = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    user_level = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    facility_id = table.Column<int>(nullable: false),
                    barangay = table.Column<int>(nullable: true),
                    muncity = table.Column<int>(nullable: true),
                    province = table.Column<int>(nullable: false),
                    designation = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    license_no = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    postfix = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOPUsers", x => x.id);
                    table.ForeignKey(
                        name: "FK_SOPUsers_Barangay",
                        column: x => x.barangay,
                        principalTable: "Barangay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOPUsers_Facility",
                        column: x => x.facility_id,
                        principalTable: "Facility",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOPUsers_Muncity",
                        column: x => x.muncity,
                        principalTable: "Muncity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOPUsers_Province",
                        column: x => x.province,
                        principalTable: "Province",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SOPForm",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sample_id = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    pcr_result = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    disease_reporting_unit_id = table.Column<int>(nullable: false),
                    datetime_collection = table.Column<DateTime>(nullable: false),
                    requested_by = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    requester_contact = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    type_specimen = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    date_onset_symptoms = table.Column<DateTime>(type: "date", nullable: false),
                    datetime_specimen_receipt = table.Column<DateTime>(nullable: false),
                    Swabber = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    date_result = table.Column<DateTime>(type: "date", nullable: false),
                    patient_id = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOPForm", x => x.id);
                    table.ForeignKey(
                        name: "FK_SOPForm_SOPUsers",
                        column: x => x.disease_reporting_unit_id,
                        principalTable: "SOPUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SOPForm_Patient",
                        column: x => x.patient_id,
                        principalTable: "Patient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResultForm",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lab_test_performed = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    test_result = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    test_results_units = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    biological_referrence = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    final_result = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    interpretation = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    comments = table.Column<string>(type: "text", nullable: true),
                    performed_by = table.Column<int>(nullable: true),
                    verified_by = table.Column<int>(nullable: true),
                    approved_by = table.Column<int>(nullable: true),
                    sop_form_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false),
                    updated_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultForm", x => x.id);
                    table.ForeignKey(
                        name: "FK_Approve_SOPUsers",
                        column: x => x.approved_by,
                        principalTable: "SOPUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultForm_SOPUsers",
                        column: x => x.created_by,
                        principalTable: "SOPUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Perform_SOPUsers",
                        column: x => x.performed_by,
                        principalTable: "SOPUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResultForm_SOPForm",
                        column: x => x.sop_form_id,
                        principalTable: "SOPForm",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Verify_SOPUsers",
                        column: x => x.verified_by,
                        principalTable: "SOPUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facility_barangay",
                table: "Facility",
                column: "barangay");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_muncity",
                table: "Facility",
                column: "muncity");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_province",
                table: "Facility",
                column: "province");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_barangay",
                table: "Patient",
                column: "barangay");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_muncity",
                table: "Patient",
                column: "muncity");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_province",
                table: "Patient",
                column: "province");

            migrationBuilder.CreateIndex(
                name: "IX_ResultForm_approved_by",
                table: "ResultForm",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_ResultForm_created_by",
                table: "ResultForm",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ResultForm_performed_by",
                table: "ResultForm",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "IX_ResultForm_sop_form_id",
                table: "ResultForm",
                column: "sop_form_id");

            migrationBuilder.CreateIndex(
                name: "IX_ResultForm_verified_by",
                table: "ResultForm",
                column: "verified_by");

            migrationBuilder.CreateIndex(
                name: "IX_SOPForm_disease_reporting_unit_id",
                table: "SOPForm",
                column: "disease_reporting_unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_SOPForm_patient_id",
                table: "SOPForm",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "IX_SOPUsers_barangay",
                table: "SOPUsers",
                column: "barangay");

            migrationBuilder.CreateIndex(
                name: "IX_SOPUsers_facility_id",
                table: "SOPUsers",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "IX_SOPUsers_muncity",
                table: "SOPUsers",
                column: "muncity");

            migrationBuilder.CreateIndex(
                name: "IX_SOPUsers_province",
                table: "SOPUsers",
                column: "province");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultForm");

            migrationBuilder.DropTable(
                name: "SOPForm");

            migrationBuilder.DropTable(
                name: "SOPUsers");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "Barangay");

            migrationBuilder.DropTable(
                name: "Muncity");

            migrationBuilder.DropTable(
                name: "Province");
        }
    }
}
