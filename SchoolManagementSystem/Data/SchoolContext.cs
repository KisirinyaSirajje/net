using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Grade> Grades { get; set; }
        
        // Uganda Curriculum Models
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<GradeScale> GradeScales { get; set; }
        public DbSet<StudentSubjectPerformance> StudentSubjectPerformances { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.StudentId).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.StudentId).IsRequired().HasMaxLength(20);
            });

            // Configure Teacher entity
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasIndex(e => e.EmployeeId).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(20);
            });

            // Configure Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.CourseCode).IsUnique();
                entity.Property(e => e.CourseCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CourseName).IsRequired().HasMaxLength(100);
                
                entity.HasOne(e => e.Teacher)
                    .WithMany(e => e.Courses)
                    .HasForeignKey(e => e.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure CourseEnrollment entity
            modelBuilder.Entity<CourseEnrollment>(entity =>
            {
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
                
                entity.HasOne(e => e.Student)
                    .WithMany(e => e.CourseEnrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Course)
                    .WithMany(e => e.CourseEnrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Attendance entity
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasIndex(e => new { e.StudentId, e.CourseId, e.AttendanceDate }).IsUnique();
                
                entity.HasOne(e => e.Student)
                    .WithMany(e => e.Attendances)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Course)
                    .WithMany(e => e.Attendances)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Grade entity
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.Property(e => e.NumericGrade).HasColumnType("decimal(5,2)");
                entity.Property(e => e.TotalPoints).HasColumnType("decimal(8,2)");
                entity.Property(e => e.EarnedPoints).HasColumnType("decimal(8,2)");
                
                entity.HasOne(e => e.Student)
                    .WithMany(e => e.Grades)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Course)
                    .WithMany(e => e.Grades)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Subject entity
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Level }).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Configure GradeScale entity
            modelBuilder.Entity<GradeScale>(entity =>
            {
                entity.HasIndex(e => new { e.Grade, e.Level }).IsUnique();
                entity.Property(e => e.Grade).IsRequired().HasMaxLength(5);
                entity.Property(e => e.GradePoint).HasColumnType("decimal(3,2)");
            });

            // Configure StudentSubjectPerformance entity
            modelBuilder.Entity<StudentSubjectPerformance>(entity =>
            {
                entity.Property(e => e.Score).HasColumnType("decimal(5,2)");
                entity.Property(e => e.GradePoint).HasColumnType("decimal(3,2)");
                
                entity.HasOne(e => e.Student)
                    .WithMany(e => e.SubjectPerformances)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Subject)
                    .WithMany(e => e.StudentPerformances)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.GradeScale)
                    .WithMany(e => e.StudentPerformances)
                    .HasForeignKey(e => e.GradeScaleId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure TeacherSubject entity
            modelBuilder.Entity<TeacherSubject>(entity =>
            {
                entity.HasIndex(e => new { e.TeacherId, e.SubjectId }).IsUnique();
                
                entity.HasOne(e => e.Teacher)
                    .WithMany(e => e.TeacherSubjects)
                    .HasForeignKey(e => e.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Subject)
                    .WithMany(e => e.TeacherSubjects)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Course-Subject relationship
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasOne(e => e.Subject)
                    .WithMany(e => e.Courses)
                    .HasForeignKey(e => e.SubjectId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Grade Scales for Uganda curriculum
            SeedGradeScales(modelBuilder);
            
            // Seed Subjects for Uganda curriculum
            SeedSubjects(modelBuilder);
            // Seed Teachers
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "j.smith@school.edu",
                    EmployeeId = "EMP001",
                    Department = "Mathematics",
                    Specialization = "Algebra & Calculus",
                    HireDate = new DateTime(2020, 8, 15),
                    PhoneNumber = "555-0101",
                    YearsOfExperience = 8
                },
                new Teacher
                {
                    Id = 2,
                    FirstName = "Sarah",
                    LastName = "Johnson",
                    Email = "s.johnson@school.edu",
                    EmployeeId = "EMP002",
                    Department = "Science",
                    Specialization = "Chemistry & Biology",
                    HireDate = new DateTime(2019, 1, 10),
                    PhoneNumber = "555-0102",
                    YearsOfExperience = 12
                },
                new Teacher
                {
                    Id = 3,
                    FirstName = "Michael",
                    LastName = "Brown",
                    Email = "m.brown@school.edu",
                    EmployeeId = "EMP003",
                    Department = "Computer Science",
                    Specialization = "Programming & Algorithms",
                    HireDate = new DateTime(2021, 9, 1),
                    PhoneNumber = "555-0103",
                    YearsOfExperience = 6
                }
            );

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    CourseCode = "MATH101",
                    CourseName = "Introduction to Algebra",
                    Description = "Basic algebraic concepts and problem solving",
                    Credits = 3,
                    TeacherId = 1,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 20),
                    MaxEnrollment = 25,
                    DaysOfWeek = "Monday, Wednesday, Friday",
                    StartTime = new TimeOnly(9, 0),
                    EndTime = new TimeOnly(10, 30),
                    Room = "Room 101"
                },
                new Course
                {
                    Id = 2,
                    CourseCode = "SCI201",
                    CourseName = "General Chemistry",
                    Description = "Introduction to chemical principles and laboratory techniques",
                    Credits = 4,
                    TeacherId = 2,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 20),
                    MaxEnrollment = 20,
                    DaysOfWeek = "Tuesday, Thursday",
                    StartTime = new TimeOnly(10, 0),
                    EndTime = new TimeOnly(12, 0),
                    Room = "Lab 201"
                },
                new Course
                {
                    Id = 3,
                    CourseCode = "CS101",
                    CourseName = "Introduction to Programming",
                    Description = "Basic programming concepts using C#",
                    Credits = 3,
                    TeacherId = 3,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2024, 12, 20),
                    MaxEnrollment = 30,
                    DaysOfWeek = "Monday, Wednesday, Friday",
                    StartTime = new TimeOnly(14, 0),
                    EndTime = new TimeOnly(15, 30),
                    Room = "Computer Lab"
                }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Wilson",
                    Email = "alice.wilson@student.edu",
                    StudentId = "STU2024001",
                    DateOfBirth = new DateTime(2005, 3, 15),
                    PhoneNumber = "555-1001",
                    EnrollmentDate = new DateTime(2024, 8, 25)
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Davis",
                    Email = "bob.davis@student.edu",
                    StudentId = "STU2024002",
                    DateOfBirth = new DateTime(2005, 7, 22),
                    PhoneNumber = "555-1002",
                    EnrollmentDate = new DateTime(2024, 8, 25)
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Carol",
                    LastName = "Martinez",
                    Email = "carol.martinez@student.edu",
                    StudentId = "STU2024003",
                    DateOfBirth = new DateTime(2004, 11, 8),
                    PhoneNumber = "555-1003",
                    EnrollmentDate = new DateTime(2024, 8, 25)
                }
            );

            // Seed Course Enrollments
            modelBuilder.Entity<CourseEnrollment>().HasData(
                new CourseEnrollment { Id = 1, StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2024, 8, 28) },
                new CourseEnrollment { Id = 2, StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2024, 8, 28) },
                new CourseEnrollment { Id = 3, StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2024, 8, 28) },
                new CourseEnrollment { Id = 4, StudentId = 2, CourseId = 2, EnrollmentDate = new DateTime(2024, 8, 28) },
                new CourseEnrollment { Id = 5, StudentId = 3, CourseId = 2, EnrollmentDate = new DateTime(2024, 8, 28) },
                new CourseEnrollment { Id = 6, StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2024, 8, 28) }
            );
        }

        private void SeedGradeScales(ModelBuilder modelBuilder)
        {
            // O-Level Grade Scale (New CBC)
            modelBuilder.Entity<GradeScale>().HasData(
                new GradeScale { Id = 1, Grade = "A", MinMark = 90, MaxMark = 100, Level = EducationLevel.OLevel, Description = "Exceptional", GradePoint = 5.0m, IsPassingGrade = true, DisplayOrder = 1 },
                new GradeScale { Id = 2, Grade = "B", MinMark = 80, MaxMark = 89, Level = EducationLevel.OLevel, Description = "Outstanding", GradePoint = 4.0m, IsPassingGrade = true, DisplayOrder = 2 },
                new GradeScale { Id = 3, Grade = "C", MinMark = 70, MaxMark = 79, Level = EducationLevel.OLevel, Description = "Satisfactory", GradePoint = 3.0m, IsPassingGrade = true, DisplayOrder = 3 },
                new GradeScale { Id = 4, Grade = "D", MinMark = 60, MaxMark = 69, Level = EducationLevel.OLevel, Description = "Basic", GradePoint = 2.0m, IsPassingGrade = true, DisplayOrder = 4 },
                new GradeScale { Id = 5, Grade = "E", MinMark = 0, MaxMark = 59, Level = EducationLevel.OLevel, Description = "Elementary", GradePoint = 0.0m, IsPassingGrade = false, DisplayOrder = 5 }
            );

            // A-Level Grade Scale (UACE)
            modelBuilder.Entity<GradeScale>().HasData(
                new GradeScale { Id = 6, Grade = "A", MinMark = 80, MaxMark = 100, Level = EducationLevel.ALevel, Description = "Distinction", GradePoint = 5.0m, IsPassingGrade = true, DisplayOrder = 1 },
                new GradeScale { Id = 7, Grade = "B", MinMark = 70, MaxMark = 79, Level = EducationLevel.ALevel, Description = "Credit", GradePoint = 4.0m, IsPassingGrade = true, DisplayOrder = 2 },
                new GradeScale { Id = 8, Grade = "C", MinMark = 60, MaxMark = 69, Level = EducationLevel.ALevel, Description = "Pass", GradePoint = 3.0m, IsPassingGrade = true, DisplayOrder = 3 },
                new GradeScale { Id = 9, Grade = "D", MinMark = 50, MaxMark = 59, Level = EducationLevel.ALevel, Description = "Weak Pass", GradePoint = 2.0m, IsPassingGrade = true, DisplayOrder = 4 },
                new GradeScale { Id = 10, Grade = "E", MinMark = 40, MaxMark = 49, Level = EducationLevel.ALevel, Description = "Fail", GradePoint = 1.0m, IsPassingGrade = false, DisplayOrder = 5 },
                new GradeScale { Id = 11, Grade = "O", MinMark = 30, MaxMark = 39, Level = EducationLevel.ALevel, Description = "Subsidiary Pass", GradePoint = 0.5m, IsPassingGrade = false, DisplayOrder = 6 },
                new GradeScale { Id = 12, Grade = "F", MinMark = 0, MaxMark = 29, Level = EducationLevel.ALevel, Description = "Fail", GradePoint = 0.0m, IsPassingGrade = false, DisplayOrder = 7 }
            );
        }

        private void SeedSubjects(ModelBuilder modelBuilder)
        {
            // O-Level Compulsory Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "English Language", Code = "ENG", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.NotApplicable, Credits = 1, Description = "Communication skills, grammar, composition and comprehension" },
                new Subject { Id = 2, Name = "Mathematics", Code = "MATH", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.NotApplicable, Credits = 1, Description = "Algebra, geometry, statistics and problem solving" },
                new Subject { Id = 3, Name = "Physics", Code = "PHY", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.Science, Credits = 1, Description = "Mechanics, waves, electricity and modern physics" },
                new Subject { Id = 4, Name = "Chemistry", Code = "CHEM", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.Science, Credits = 1, Description = "Atomic structure, chemical bonding and reactions" },
                new Subject { Id = 5, Name = "Biology", Code = "BIO", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.Science, Credits = 1, Description = "Cell biology, genetics, ecology and human biology" },
                new Subject { Id = 6, Name = "History & Political Education", Code = "HIST", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.Arts, Credits = 1, Description = "World history, Ugandan history and political systems" },
                new Subject { Id = 7, Name = "Geography", Code = "GEOG", Level = EducationLevel.OLevel, Type = SubjectType.Compulsory, Stream = Models.Stream.NotApplicable, Credits = 1, Description = "Physical and human geography of Uganda and the world" }
            );

            // O-Level Elective Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 8, Name = "Literature in English", Code = "LIT", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 1, Description = "Poetry, prose, drama and literary analysis" },
                new Subject { Id = 9, Name = "Christian Religious Education", Code = "CRE", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 1, Description = "Biblical studies, Christian doctrine and ethics" },
                new Subject { Id = 10, Name = "Islamic Religious Education", Code = "IRE", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 1, Description = "Quranic studies, Islamic principles and history" },
                new Subject { Id = 11, Name = "Agriculture", Code = "AGRIC", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Science, Credits = 1, Description = "Crop production, animal husbandry and agricultural economics" },
                new Subject { Id = 12, Name = "ICT/Computer Studies", Code = "ICT", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Technical, Credits = 1, Description = "Computer literacy, programming basics and digital citizenship" },
                new Subject { Id = 13, Name = "Entrepreneurship", Code = "ENTR", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Business, Credits = 1, Description = "Business skills, innovation and economic principles" },
                new Subject { Id = 14, Name = "Fine Art", Code = "ART", Level = EducationLevel.OLevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 1, Description = "Drawing, painting, sculpture and art history" },
                new Subject { Id = 15, Name = "Physical Education", Code = "PE", Level = EducationLevel.OLevel, Type = SubjectType.Optional, Stream = Models.Stream.NotApplicable, Credits = 1, Description = "Sports, fitness and health education" }
            );

            // A-Level Science Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 16, Name = "A-Level Mathematics", Code = "A-MATH", Level = EducationLevel.ALevel, Type = SubjectType.Compulsory, Stream = Models.Stream.Science, Credits = 2, Description = "Advanced algebra, calculus, statistics and mechanics" },
                new Subject { Id = 17, Name = "A-Level Physics", Code = "A-PHY", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Science, Credits = 2, Description = "Advanced mechanics, thermodynamics, electromagnetism and quantum physics" },
                new Subject { Id = 18, Name = "A-Level Chemistry", Code = "A-CHEM", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Science, Credits = 2, Description = "Organic chemistry, physical chemistry and analytical techniques" },
                new Subject { Id = 19, Name = "A-Level Biology", Code = "A-BIO", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Science, Credits = 2, Description = "Advanced biology, biochemistry, genetics and ecology" },
                new Subject { Id = 20, Name = "Subsidiary Mathematics", Code = "S-MATH", Level = EducationLevel.ALevel, Type = SubjectType.Optional, Stream = Models.Stream.Science, Credits = 1, Description = "Applied mathematics for non-mathematics majors" }
            );

            // A-Level Arts/Humanities Subjects
            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 21, Name = "A-Level History", Code = "A-HIST", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 2, Description = "Advanced historical analysis and research methods" },
                new Subject { Id = 22, Name = "A-Level Geography", Code = "A-GEOG", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 2, Description = "Advanced physical and human geography with fieldwork" },
                new Subject { Id = 23, Name = "A-Level Economics", Code = "A-ECON", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Business, Credits = 2, Description = "Microeconomics, macroeconomics and economic development" },
                new Subject { Id = 24, Name = "A-Level Literature", Code = "A-LIT", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 2, Description = "Advanced literary criticism and comparative literature" },
                new Subject { Id = 25, Name = "A-Level CRE", Code = "A-CRE", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 2, Description = "Advanced Christian theology and comparative religion" },
                new Subject { Id = 26, Name = "A-Level IRE", Code = "A-IRE", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Arts, Credits = 2, Description = "Advanced Islamic studies and jurisprudence" },
                new Subject { Id = 27, Name = "A-Level Entrepreneurship", Code = "A-ENTR", Level = EducationLevel.ALevel, Type = SubjectType.Elective, Stream = Models.Stream.Business, Credits = 2, Description = "Advanced business management and innovation" }
            );
        }
    }
}
