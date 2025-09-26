namespace SchoolManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int TotalEnrollments { get; set; }
        
        public List<Student> RecentStudents { get; set; } = new List<Student>();
        public List<Course> PopularCourses { get; set; } = new List<Course>();
        public List<Grade> RecentGrades { get; set; } = new List<Grade>();
        public AttendanceSummary TodayAttendance { get; set; } = new AttendanceSummary();
        
        public decimal? AverageGPA
        {
            get
            {
                if (RecentGrades.Any() && RecentGrades.Any(g => g.NumericGrade.HasValue))
                {
                    return Math.Round(RecentGrades.Where(g => g.NumericGrade.HasValue)
                        .Average(g => g.NumericGrade!.Value), 2);
                }
                return null;
            }
        }
    }

    public class AttendanceSummary
    {
        public int TotalRecorded { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Excused { get; set; }
        
        public decimal AttendanceRate
        {
            get
            {
                if (TotalRecorded == 0) return 0;
                return Math.Round(((decimal)(Present + Late) / TotalRecorded) * 100, 1);
            }
        }
    }
}