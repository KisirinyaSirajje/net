using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Student ID is required")]
        [StringLength(20, ErrorMessage = "Student ID cannot exceed 20 characters")]
        public string StudentId { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [Display(Name = "Education Level")]
        public EducationLevel CurrentLevel { get; set; } = EducationLevel.OLevel;

        [Display(Name = "Class/Form")]
        [StringLength(10)]
        public string? CurrentClass { get; set; } // e.g., S1, S2, S3, S4, S5, S6

        [Display(Name = "Stream")]
        public Stream Stream { get; set; } = Stream.NotApplicable;

        [StringLength(20)]
        [Display(Name = "Index Number")]
        public string? IndexNumber { get; set; } // For national exams

        [Display(Name = "Academic Year")]
        [StringLength(10)]
        public string? AcademicYear { get; set; }

        [StringLength(100)]
        [Display(Name = "Guardian/Parent Name")]
        public string? GuardianName { get; set; }

        [StringLength(15)]
        [Display(Name = "Guardian Phone")]
        public string? GuardianPhone { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public virtual ICollection<StudentSubjectPerformance> SubjectPerformances { get; set; } = new List<StudentSubjectPerformance>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";

        // GPA calculation
        public decimal? GPA
        {
            get
            {
                var validGrades = Grades.Where(g => g.NumericGrade.HasValue).ToList();
                if (!validGrades.Any()) return null;
                
                return Math.Round(validGrades.Average(g => g.NumericGrade!.Value), 2);
            }
        }
    }
}