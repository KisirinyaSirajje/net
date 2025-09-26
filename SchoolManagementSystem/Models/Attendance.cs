using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Attendance Date")]
        public DateTime AttendanceDate { get; set; } = DateTime.Today;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Present"; // Present, Absent, Late, Excused

        [StringLength(200)]
        public string? Notes { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Check-in Time")]
        public TimeOnly? CheckInTime { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        // Helper properties
        public bool IsPresent => Status == "Present" || Status == "Late";
        public string StatusColor => Status switch
        {
            "Present" => "success",
            "Late" => "warning", 
            "Excused" => "info",
            "Absent" => "danger",
            _ => "secondary"
        };
    }
}