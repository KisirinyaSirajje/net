using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Teacher
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

        [Required(ErrorMessage = "Employee ID is required")]
        [StringLength(20, ErrorMessage = "Employee ID cannot exceed 20 characters")]
        public string EmployeeId { get; set; } = string.Empty;

        [StringLength(15)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        [StringLength(100)]
        public string? Specialization { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? Address { get; set; }

        [Range(0, 50)]
        [Display(Name = "Years of Experience")]
        public int? YearsOfExperience { get; set; }

        [Display(Name = "Highest Qualification")]
        public QualificationLevel HighestQualification { get; set; } = QualificationLevel.BachelorsDegree;

        [Display(Name = "Teaching License Number")]
        [StringLength(50)]
        public string? TeachingLicenseNumber { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";

        // Number of courses taught
        public int CourseCount => Courses?.Count ?? 0;
    }
}