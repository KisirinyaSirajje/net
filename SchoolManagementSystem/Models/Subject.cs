using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject name is required")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters")]
        [Display(Name = "Subject Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Subject Code")]
        public string? Code { get; set; }

        [Required]
        [Display(Name = "Education Level")]
        public EducationLevel Level { get; set; }

        [Required]
        [Display(Name = "Subject Type")]
        public SubjectType Type { get; set; }

        [Display(Name = "Stream")]
        public Stream Stream { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Credits/Hours")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        public int Credits { get; set; } = 1;

        // Navigation properties
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<StudentSubjectPerformance> StudentPerformances { get; set; } = new List<StudentSubjectPerformance>();
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

        // Computed properties
        public string FullName => !string.IsNullOrEmpty(Code) ? $"{Code} - {Name}" : Name;
        public string LevelAndStream => Stream != Stream.NotApplicable ? $"{Level} ({Stream})" : Level.ToString();
    }

    public enum EducationLevel
    {
        [Display(Name = "O-Level (S1-S4)")]
        OLevel,
        
        [Display(Name = "A-Level (S5-S6)")]
        ALevel
    }

    public enum SubjectType
    {
        [Display(Name = "Compulsory")]
        Compulsory,
        
        [Display(Name = "Elective")]
        Elective,
        
        [Display(Name = "Optional")]
        Optional
    }

    public enum Stream
    {
        [Display(Name = "Not Applicable")]
        NotApplicable,
        
        [Display(Name = "Science/STEM")]
        Science,
        
        [Display(Name = "Arts/Humanities")]
        Arts,
        
        [Display(Name = "Business/Commerce")]
        Business,
        
        [Display(Name = "Technical")]
        Technical
    }
}