using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UgandaCurriculumModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HighestQualification",
                table: "Teachers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teachers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeachingLicenseNumber",
                table: "Teachers",
                type: "TEXT",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Students",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentClass",
                table: "Students",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentLevel",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "Students",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianPhone",
                table: "Students",
                type: "TEXT",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndexNumber",
                table: "Students",
                type: "TEXT",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stream",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYear",
                table: "Courses",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Courses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Term",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GradeScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grade = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    MinMark = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxMark = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    GradePoint = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    IsPassingGrade = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Stream = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Credits = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentSubjectPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcademicYear = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Term = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessmentType = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    GradeScaleId = table.Column<int>(type: "INTEGER", nullable: true),
                    LetterGrade = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    GradePoint = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    ResultStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AssessmentDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsFinalGrade = table.Column<bool>(type: "INTEGER", nullable: false),
                    WeightPercentage = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubjectPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSubjectPerformances_GradeScales_GradeScaleId",
                        column: x => x.GradeScaleId,
                        principalTable: "GradeScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StudentSubjectPerformances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentSubjectPerformances_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualificationLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrimarySubject = table.Column<bool>(type: "INTEGER", nullable: false),
                    CertificationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    QualificationDetails = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherSubjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AcademicYear", "SubjectId", "Term" },
                values: new object[] { "2025", null, 0 });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AcademicYear", "SubjectId", "Term" },
                values: new object[] { "2025", null, 0 });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AcademicYear", "SubjectId", "Term" },
                values: new object[] { "2025", null, 0 });

            migrationBuilder.InsertData(
                table: "GradeScales",
                columns: new[] { "Id", "Description", "DisplayOrder", "Grade", "GradePoint", "IsPassingGrade", "Level", "MaxMark", "MinMark" },
                values: new object[,]
                {
                    { 1, "Exceptional", 1, "A", 5.0m, true, 0, 100, 90 },
                    { 2, "Outstanding", 2, "B", 4.0m, true, 0, 89, 80 },
                    { 3, "Satisfactory", 3, "C", 3.0m, true, 0, 79, 70 },
                    { 4, "Basic", 4, "D", 2.0m, true, 0, 69, 60 },
                    { 5, "Elementary", 5, "E", 0.0m, false, 0, 59, 0 },
                    { 6, "Distinction", 1, "A", 5.0m, true, 1, 100, 80 },
                    { 7, "Credit", 2, "B", 4.0m, true, 1, 79, 70 },
                    { 8, "Pass", 3, "C", 3.0m, true, 1, 69, 60 },
                    { 9, "Weak Pass", 4, "D", 2.0m, true, 1, 59, 50 },
                    { 10, "Fail", 5, "E", 1.0m, false, 1, 49, 40 },
                    { 11, "Subsidiary Pass", 6, "O", 0.5m, false, 1, 39, 30 },
                    { 12, "Fail", 7, "F", 0.0m, false, 1, 29, 0 }
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AcademicYear", "CurrentClass", "CurrentLevel", "GuardianName", "GuardianPhone", "IndexNumber", "IsActive", "Stream" },
                values: new object[] { null, null, 0, null, null, null, true, 0 });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AcademicYear", "CurrentClass", "CurrentLevel", "GuardianName", "GuardianPhone", "IndexNumber", "IsActive", "Stream" },
                values: new object[] { null, null, 0, null, null, null, true, 0 });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AcademicYear", "CurrentClass", "CurrentLevel", "GuardianName", "GuardianPhone", "IndexNumber", "IsActive", "Stream" },
                values: new object[] { null, null, 0, null, null, null, true, 0 });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Code", "Credits", "Description", "IsActive", "Level", "Name", "Stream", "Type" },
                values: new object[,]
                {
                    { 1, "ENG", 1, "Communication skills, grammar, composition and comprehension", true, 0, "English Language", 0, 0 },
                    { 2, "MATH", 1, "Algebra, geometry, statistics and problem solving", true, 0, "Mathematics", 0, 0 },
                    { 3, "PHY", 1, "Mechanics, waves, electricity and modern physics", true, 0, "Physics", 1, 0 },
                    { 4, "CHEM", 1, "Atomic structure, chemical bonding and reactions", true, 0, "Chemistry", 1, 0 },
                    { 5, "BIO", 1, "Cell biology, genetics, ecology and human biology", true, 0, "Biology", 1, 0 },
                    { 6, "HIST", 1, "World history, Ugandan history and political systems", true, 0, "History & Political Education", 2, 0 },
                    { 7, "GEOG", 1, "Physical and human geography of Uganda and the world", true, 0, "Geography", 0, 0 },
                    { 8, "LIT", 1, "Poetry, prose, drama and literary analysis", true, 0, "Literature in English", 2, 1 },
                    { 9, "CRE", 1, "Biblical studies, Christian doctrine and ethics", true, 0, "Christian Religious Education", 2, 1 },
                    { 10, "IRE", 1, "Quranic studies, Islamic principles and history", true, 0, "Islamic Religious Education", 2, 1 },
                    { 11, "AGRIC", 1, "Crop production, animal husbandry and agricultural economics", true, 0, "Agriculture", 1, 1 },
                    { 12, "ICT", 1, "Computer literacy, programming basics and digital citizenship", true, 0, "ICT/Computer Studies", 4, 1 },
                    { 13, "ENTR", 1, "Business skills, innovation and economic principles", true, 0, "Entrepreneurship", 3, 1 },
                    { 14, "ART", 1, "Drawing, painting, sculpture and art history", true, 0, "Fine Art", 2, 1 },
                    { 15, "PE", 1, "Sports, fitness and health education", true, 0, "Physical Education", 0, 2 },
                    { 16, "A-MATH", 2, "Advanced algebra, calculus, statistics and mechanics", true, 1, "A-Level Mathematics", 1, 0 },
                    { 17, "A-PHY", 2, "Advanced mechanics, thermodynamics, electromagnetism and quantum physics", true, 1, "A-Level Physics", 1, 1 },
                    { 18, "A-CHEM", 2, "Organic chemistry, physical chemistry and analytical techniques", true, 1, "A-Level Chemistry", 1, 1 },
                    { 19, "A-BIO", 2, "Advanced biology, biochemistry, genetics and ecology", true, 1, "A-Level Biology", 1, 1 },
                    { 20, "S-MATH", 1, "Applied mathematics for non-mathematics majors", true, 1, "Subsidiary Mathematics", 1, 2 },
                    { 21, "A-HIST", 2, "Advanced historical analysis and research methods", true, 1, "A-Level History", 2, 1 },
                    { 22, "A-GEOG", 2, "Advanced physical and human geography with fieldwork", true, 1, "A-Level Geography", 2, 1 },
                    { 23, "A-ECON", 2, "Microeconomics, macroeconomics and economic development", true, 1, "A-Level Economics", 3, 1 },
                    { 24, "A-LIT", 2, "Advanced literary criticism and comparative literature", true, 1, "A-Level Literature", 2, 1 },
                    { 25, "A-CRE", 2, "Advanced Christian theology and comparative religion", true, 1, "A-Level CRE", 2, 1 },
                    { 26, "A-IRE", 2, "Advanced Islamic studies and jurisprudence", true, 1, "A-Level IRE", 2, 1 },
                    { 27, "A-ENTR", 2, "Advanced business management and innovation", true, 1, "A-Level Entrepreneurship", 3, 1 }
                });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HighestQualification", "IsActive", "TeachingLicenseNumber" },
                values: new object[] { 2, true, null });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "HighestQualification", "IsActive", "TeachingLicenseNumber" },
                values: new object[] { 2, true, null });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "HighestQualification", "IsActive", "TeachingLicenseNumber" },
                values: new object[] { 2, true, null });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeScales_Grade_Level",
                table: "GradeScales",
                columns: new[] { "Grade", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectPerformances_GradeScaleId",
                table: "StudentSubjectPerformances",
                column: "GradeScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectPerformances_StudentId",
                table: "StudentSubjectPerformances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectPerformances_SubjectId",
                table: "StudentSubjectPerformances",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Name_Level",
                table: "Subjects",
                columns: new[] { "Name", "Level" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_SubjectId",
                table: "TeacherSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSubjects_TeacherId_SubjectId",
                table: "TeacherSubjects",
                columns: new[] { "TeacherId", "SubjectId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Subjects_SubjectId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "StudentSubjectPerformances");

            migrationBuilder.DropTable(
                name: "TeacherSubjects");

            migrationBuilder.DropTable(
                name: "GradeScales");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SubjectId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "HighestQualification",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TeachingLicenseNumber",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CurrentClass",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CurrentLevel",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "GuardianPhone",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IndexNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Stream",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Term",
                table: "Courses");
        }
    }
}
