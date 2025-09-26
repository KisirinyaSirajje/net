using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SchoolContext _context;

    public HomeController(ILogger<HomeController> logger, SchoolContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var dashboardData = new DashboardViewModel
        {
            TotalStudents = await _context.Students.CountAsync(),
            TotalTeachers = await _context.Teachers.CountAsync(),
            TotalCourses = await _context.Courses.CountAsync(),
            ActiveCourses = await _context.Courses
                .Where(c => c.EndDate == null || c.EndDate > DateTime.Now)
                .CountAsync(),
            TotalEnrollments = await _context.CourseEnrollments.CountAsync(),
            
            // Recent students (last 30 days)
            RecentStudents = await _context.Students
                .Where(s => s.EnrollmentDate >= DateTime.Now.AddDays(-30))
                .OrderByDescending(s => s.EnrollmentDate)
                .Take(5)
                .ToListAsync(),
                
            // Popular courses (by enrollment count)
            PopularCourses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.CourseEnrollments)
                .OrderByDescending(c => c.CourseEnrollments.Count)
                .Take(5)
                .ToListAsync(),
                
            // Today's attendance summary
            TodayAttendance = await GetTodayAttendanceSummary(),
            
            // Recent grades
            RecentGrades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .OrderByDescending(g => g.GradeDate)
                .Take(10)
                .ToListAsync()
        };

        return View(dashboardData);
    }

    private async Task<AttendanceSummary> GetTodayAttendanceSummary()
    {
        var today = DateTime.Today;
        var todayAttendances = await _context.Attendances
            .Where(a => a.AttendanceDate.Date == today)
            .ToListAsync();

        return new AttendanceSummary
        {
            TotalRecorded = todayAttendances.Count,
            Present = todayAttendances.Count(a => a.Status == "Present"),
            Absent = todayAttendances.Count(a => a.Status == "Absent"),
            Late = todayAttendances.Count(a => a.Status == "Late"),
            Excused = todayAttendances.Count(a => a.Status == "Excused")
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
