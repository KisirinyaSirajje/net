using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        public int Credits { get; set; } = 3;

        [Required]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Display(Name = "Subject")]
        public int? SubjectId { get; set; }

        [Display(Name = "Academic Year")]
        [StringLength(10)]
        public string AcademicYear { get; set; } = DateTime.Now.Year.ToString();

        [Display(Name = "Term")]
        public Term Term { get; set; } = Term.Term1;

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Range(1, 200, ErrorMessage = "Maximum enrollment must be between 1 and 200")]
        [Display(Name = "Maximum Enrollment")]
        public int MaxEnrollment { get; set; } = 30;

        // Schedule information
        [StringLength(50)]
        [Display(Name = "Days of Week")]
        public string? DaysOfWeek { get; set; } // e.g., "Monday, Wednesday, Friday"

        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeOnly? StartTime { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeOnly? EndTime { get; set; }

        [StringLength(50)]
        public string? Room { get; set; }

        // Navigation properties
        [ForeignKey("TeacherId")]
        public virtual Teacher? Teacher { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }
        
        public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

        // Computed properties
        public string FullCourseName => $"{CourseCode} - {CourseName}";
        public int EnrolledCount => CourseEnrollments?.Count ?? 0;
        public bool IsActive => EndDate == null || EndDate > DateTime.Now;
        public string Schedule => $"{DaysOfWeek} {StartTime?.ToString("HH:mm")} - {EndTime?.ToString("HH:mm")} ({Room})";
    }
}