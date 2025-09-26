using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class TeacherSubject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Display(Name = "Qualification Level")]
        public QualificationLevel QualificationLevel { get; set; }

        [Display(Name = "Years of Experience")]
        [Range(0, 50)]
        public int YearsOfExperience { get; set; } = 0;

        [Display(Name = "Is Primary Subject")]
        public bool IsPrimarySubject { get; set; } = false;

        [Display(Name = "Certification Date")]
        [DataType(DataType.Date)]
        public DateTime? CertificationDate { get; set; }

        [StringLength(200)]
        [Display(Name = "Qualification Details")]
        public string? QualificationDetails { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Assigned Date")]
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("TeacherId")]
        public virtual Teacher? Teacher { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        // Computed properties
        public string TeacherName => Teacher != null ? $"{Teacher.FirstName} {Teacher.LastName}" : "Unknown";
        public string SubjectName => Subject?.Name ?? "Unknown";
        public string QualificationSummary => $"{QualificationLevel} ({YearsOfExperience} years)";
    }

    public enum QualificationLevel
    {
        [Display(Name = "Certificate")]
        Certificate,
        
        [Display(Name = "Diploma")]
        Diploma,
        
        [Display(Name = "Bachelor's Degree")]
        BachelorsDegree,
        
        [Display(Name = "Master's Degree")]
        MastersDegree,
        
        [Display(Name = "PhD/Doctorate")]
        PhD,
        
        [Display(Name = "Professional Certificate")]
        ProfessionalCertificate,
        
        [Display(Name = "Other")]
        Other
    }
}