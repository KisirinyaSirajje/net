using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolManagementSystem.Models
{
    public class GradeScale
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Grade")]
        public string Grade { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Minimum Mark")]
        [Range(0, 100, ErrorMessage = "Minimum mark must be between 0 and 100")]
        public int MinMark { get; set; }

        [Required]
        [Display(Name = "Maximum Mark")]
        [Range(0, 100, ErrorMessage = "Maximum mark must be between 0 and 100")]
        public int MaxMark { get; set; }

        [Required]
        [Display(Name = "Education Level")]
        public EducationLevel Level { get; set; }

        [StringLength(100)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Grade Point")]
        [Column(TypeName = "decimal(3,2)")]
        public decimal GradePoint { get; set; }

        [Display(Name = "Is Passing Grade")]
        public bool IsPassingGrade { get; set; } = true;

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<StudentSubjectPerformance> StudentPerformances { get; set; } = new List<StudentSubjectPerformance>();

        // Computed properties
        public string FullDescription => !string.IsNullOrEmpty(Description) 
            ? $"{Grade} ({MinMark}-{MaxMark}%) - {Description}" 
            : $"{Grade} ({MinMark}-{MaxMark}%)";

        public string LevelDisplay => Level == EducationLevel.OLevel ? "O-Level" : "A-Level";
    }

    public enum UgandaGrade
    {
        // O-Level Grades (UCE - New CBC)
        [Display(Name = "A - Exceptional")]
        A_Exceptional,
        
        [Display(Name = "B - Outstanding")]
        B_Outstanding,
        
        [Display(Name = "C - Satisfactory")]
        C_Satisfactory,
        
        [Display(Name = "D - Basic")]
        D_Basic,
        
        [Display(Name = "E - Elementary")]
        E_Elementary,

        // A-Level Grades (UACE)
        [Display(Name = "A - Distinction")]
        A_Distinction,
        
        [Display(Name = "B - Credit")]
        B_Credit,
        
        [Display(Name = "C - Pass")]
        C_Pass,
        
        [Display(Name = "D - Weak Pass")]
        D_WeakPass,
        
        [Display(Name = "E - Fail")]
        E_Fail,
        
        [Display(Name = "O - Subsidiary Pass")]
        O_SubsidiaryPass,
        
        [Display(Name = "F - Fail")]
        F_Fail
    }

    public enum ResultStatus
    {
        [Display(Name = "Draft")]
        Draft,
        
        [Display(Name = "Submitted")]
        Submitted,
        
        [Display(Name = "Approved")]
        Approved,
        
        [Display(Name = "Published")]
        Published,
        
        [Display(Name = "Pending Assessment")]
        Pending,
        
        [Display(Name = "Incomplete")]
        Incomplete,
        
        [Display(Name = "Result 1 - Qualifies for Certificate")]
        Result1_Qualified,
        
        [Display(Name = "Result 2 - Does Not Qualify (Missing Requirements)")]
        Result2_NotQualified,
        
        [Display(Name = "Result 3 - Below Basic Competency")]
        Result3_BelowBasic
    }
}