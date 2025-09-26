using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Assignment/Exam")]
        public string AssignmentName { get; set; } = string.Empty;

        [StringLength(50)]
        [Display(Name = "Grade Type")]
        public string GradeType { get; set; } = "Assignment"; // Assignment, Quiz, Midterm, Final, Project

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        [Display(Name = "Grade (%)")]
        public decimal? NumericGrade { get; set; }

        [StringLength(5)]
        [Display(Name = "Letter Grade")]
        public string? LetterGrade { get; set; }

        [Range(0, 100, ErrorMessage = "Points must be between 0 and 100")]
        [Display(Name = "Total Points")]
        public decimal? TotalPoints { get; set; }

        [Range(0, 100, ErrorMessage = "Earned points cannot exceed total points")]
        [Display(Name = "Earned Points")]
        public decimal? EarnedPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Grade Date")]
        public DateTime GradeDate { get; set; } = DateTime.Now;

        [StringLength(300)]
        public string? Comments { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        // Computed properties
        public string GetLetterGrade()
        {
            if (!NumericGrade.HasValue) return "N/A";
            
            return NumericGrade.Value switch
            {
                >= 97 => "A+",
                >= 93 => "A",
                >= 90 => "A-",
                >= 87 => "B+",
                >= 83 => "B",
                >= 80 => "B-",
                >= 77 => "C+",
                >= 73 => "C",
                >= 70 => "C-",
                >= 67 => "D+",
                >= 65 => "D",
                _ => "F"
            };
        }

        public string GradeColor => GetLetterGrade() switch
        {
            "A+" or "A" or "A-" => "success",
            "B+" or "B" or "B-" => "info",
            "C+" or "C" or "C-" => "warning",
            "D+" or "D" => "secondary",
            "F" => "danger",
            _ => "light"
        };

        public decimal? CalculatePercentage()
        {
            if (TotalPoints.HasValue && EarnedPoints.HasValue && TotalPoints.Value > 0)
            {
                return Math.Round((EarnedPoints.Value / TotalPoints.Value) * 100, 2);
            }
            return NumericGrade;
        }
    }
}