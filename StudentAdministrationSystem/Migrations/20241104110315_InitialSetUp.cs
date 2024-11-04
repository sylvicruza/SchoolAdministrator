using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentAdministrationSystem.Migrations
{
    public partial class InitialSetUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgrammeType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    NumberOfCourseModule = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DegreeProgramme",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 6, nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ProgrammeTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DegreeProgramme", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DegreeProgramme_ProgrammeType_ProgrammeTypeId",
                        column: x => x.ProgrammeTypeId,
                        principalTable: "ProgrammeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNumber = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    ContactNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Cohort = table.Column<string>(nullable: true),
                    DegreeProgrammeId = table.Column<int>(nullable: false),
                    EnrollmentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_DegreeProgramme_DegreeProgrammeId",
                        column: x => x.DegreeProgrammeId,
                        principalTable: "DegreeProgramme",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseModule",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseTitle = table.Column<string>(nullable: false),
                    CourseCode = table.Column<string>(maxLength: 5, nullable: false),
                    CourseType = table.Column<string>(nullable: true),
                    DegreeProgrammeId = table.Column<int>(nullable: false),
                    Marks = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseModule_DegreeProgramme_DegreeProgrammeId",
                        column: x => x.DegreeProgrammeId,
                        principalTable: "DegreeProgramme",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseModule_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    AssessmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentName = table.Column<string>(nullable: true),
                    Marks = table.Column<float>(nullable: false),
                    CourseModuleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.AssessmentID);
                    table.ForeignKey(
                        name: "FK_Assessment_CourseModule_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "CourseModule",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(nullable: false),
                    DegreeProgrammeId = table.Column<int>(nullable: false),
                    CourseModuleId = table.Column<int>(nullable: false),
                    AssessmentId = table.Column<int>(nullable: true),
                    Mark = table.Column<float>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollment_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
                        principalColumn: "AssessmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollment_CourseModule_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "CourseModule",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_DegreeProgramme_DegreeProgrammeId",
                        column: x => x.DegreeProgrammeId,
                        principalTable: "DegreeProgramme",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Enrollment_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_CourseModuleId",
                table: "Assessment",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModule_DegreeProgrammeId",
                table: "CourseModule",
                column: "DegreeProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModule_StudentId",
                table: "CourseModule",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_DegreeProgramme_ProgrammeTypeId",
                table: "DegreeProgramme",
                column: "ProgrammeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_AssessmentId",
                table: "Enrollment",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseModuleId",
                table: "Enrollment",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_DegreeProgrammeId",
                table: "Enrollment",
                column: "DegreeProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_StudentId",
                table: "Enrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_DegreeProgrammeId",
                table: "Student",
                column: "DegreeProgrammeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropTable(
                name: "CourseModule");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "DegreeProgramme");

            migrationBuilder.DropTable(
                name: "ProgrammeType");
        }
    }
}
