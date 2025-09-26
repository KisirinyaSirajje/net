using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    StudentId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    Department = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Specialization = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    HireDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CourseName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Credits = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MaxEnrollment = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    Room = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CheckInTime = table.Column<TimeOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignmentName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GradeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NumericGrade = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    LetterGrade = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
                    TotalPoints = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    EarnedPoints = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    GradeDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "DateOfBirth", "Email", "EnrollmentDate", "FirstName", "LastName", "PhoneNumber", "StudentId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2005, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice.wilson@student.edu", new DateTime(2024, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice", "Wilson", "555-1001", "STU2024001" },
                    { 2, null, new DateTime(2005, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob.davis@student.edu", new DateTime(2024, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob", "Davis", "555-1002", "STU2024002" },
                    { 3, null, new DateTime(2004, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "carol.martinez@student.edu", new DateTime(2024, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carol", "Martinez", "555-1003", "STU2024003" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Address", "Department", "Email", "EmployeeId", "FirstName", "HireDate", "LastName", "PhoneNumber", "Specialization", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, null, "Mathematics", "j.smith@school.edu", "EMP001", "John", new DateTime(2020, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Smith", "555-0101", "Algebra & Calculus", 8 },
                    { 2, null, "Science", "s.johnson@school.edu", "EMP002", "Sarah", new DateTime(2019, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johnson", "555-0102", "Chemistry & Biology", 12 },
                    { 3, null, "Computer Science", "m.brown@school.edu", "EMP003", "Michael", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brown", "555-0103", "Programming & Algorithms", 6 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "CourseName", "Credits", "DaysOfWeek", "Description", "EndDate", "EndTime", "MaxEnrollment", "Room", "StartDate", "StartTime", "TeacherId" },
                values: new object[,]
                {
                    { 1, "MATH101", "Introduction to Algebra", 3, "Monday, Wednesday, Friday", "Basic algebraic concepts and problem solving", new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(10, 30, 0), 25, "Room 101", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(9, 0, 0), 1 },
                    { 2, "SCI201", "General Chemistry", 4, "Tuesday, Thursday", "Introduction to chemical principles and laboratory techniques", new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(12, 0, 0), 20, "Lab 201", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(10, 0, 0), 2 },
                    { 3, "CS101", "Introduction to Programming", 3, "Monday, Wednesday, Friday", "Basic programming concepts using C#", new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(15, 30, 0), 30, "Computer Lab", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeOnly(14, 0, 0), 3 }
                });

            migrationBuilder.InsertData(
                table: "CourseEnrollments",
                columns: new[] { "Id", "CourseId", "EnrollmentDate", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1 },
                    { 2, 3, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1 },
                    { 3, 1, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 2 },
                    { 4, 2, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 2 },
                    { 5, 2, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 3 },
                    { 6, 3, new DateTime(2024, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_CourseId",
                table: "Attendances",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId_CourseId_AttendanceDate",
                table: "Attendances",
                columns: new[] { "StudentId", "CourseId", "AttendanceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId",
                table: "CourseEnrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StudentId_CourseId",
                table: "CourseEnrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCode",
                table: "Courses",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CourseId",
                table: "Grades",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentId",
                table: "Students",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_EmployeeId",
                table: "Teachers",
                column: "EmployeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "CourseEnrollments");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
