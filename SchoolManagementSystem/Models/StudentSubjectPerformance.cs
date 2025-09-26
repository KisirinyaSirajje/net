using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolManagementSystem.Models
{
    public class StudentSubjectPerformance
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        [StringLength(10)]
        public string AcademicYear { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Term/Semester")]
        public Term Term { get; set; }

        [Display(Name = "Assessment Type")]
        public AssessmentType AssessmentType { get; set; }

        [Display(Name = "Score/Mark")]
        [Range(0, 100, ErrorMessage = "Score must be between 0 and 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        [Display(Name = "Grade")]
        public int? GradeScaleId { get; set; }

        [StringLength(5)]
        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        [Display(Name = "Grade Point")]
        [Column(TypeName = "decimal(3,2)")]
        public decimal? GradePoint { get; set; }

        [Display(Name = "Result Status")]
        public ResultStatus ResultStatus { get; set; } = ResultStatus.Pending;

        [StringLength(500)]
        [Display(Name = "Comments/Remarks")]
        public string? Comments { get; set; }

        [Display(Name = "Assessment Date")]
        [DataType(DataType.Date)]
        public DateTime? AssessmentDate { get; set; }

        [Display(Name = "Submitted Date")]
        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        [Display(Name = "Is Final Grade")]
        public bool IsFinalGrade { get; set; } = false;

        [Display(Name = "Weight Percentage")]
        [Range(0, 100)]
        public int WeightPercentage { get; set; } = 100;

        // Navigation properties
        [ForeignKey("StudentId")]
        [JsonIgnore]
        public virtual Student? Student { get; set; }

        [ForeignKey("SubjectId")]
        [JsonIgnore]
        public virtual Subject? Subject { get; set; }

        [ForeignKey("GradeScaleId")]
        public virtual GradeScale? GradeScale { get; set; }

        // Computed properties
        public string StudentName => Student != null ? $"{Student.FirstName} {Student.LastName}" : "Unknown";
        public string SubjectName => Subject?.Name ?? "Unknown";
        public string PerformanceSummary => Score.HasValue ? $"{LetterGrade} ({Score:F1}%)" : "Not Assessed";
        public bool IsPassing => GradeScale?.IsPassingGrade ?? false;
        public string AcademicPeriod => $"{AcademicYear} - {Term}";
    }

    public enum Term
    {
        [Display(Name = "Term 1")]
        Term1,

        [Display(Name = "Term 2")]
        Term2,

        [Display(Name = "Term 3")]
        Term3,

        [Display(Name = "Semester 1")]
        Semester1,

        [Display(Name = "Semester 2")]
        Semester2,

        [Display(Name = "Annual")]
        Annual
    }

    public enum AssessmentType
    {
        [Display(Name = "Continuous Assessment")]
        ContinuousAssessment,

        [Display(Name = "Mid-Term Exam")]
        MidtermExam,

        [Display(Name = "Final Exam")]
        FinalExam,

        [Display(Name = "Mock Exam")]
        MockExam,

        [Display(Name = "UCE (O-Level Final)")]
        UCEExam,

        [Display(Name = "UACE (A-Level Final)")]
        UACEExam,

        [Display(Name = "Project Work")]
        ProjectWork,

        [Display(Name = "Practical Assessment")]
        PracticalAssessment,

        [Display(Name = "Coursework")]
        Coursework
    }
}