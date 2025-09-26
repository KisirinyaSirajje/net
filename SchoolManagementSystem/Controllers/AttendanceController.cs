using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly SchoolContext _context;

        public AttendanceController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Attendance
        public async Task<IActionResult> Index(DateTime? date, int? courseId)
        {
            var attendanceDate = date ?? DateTime.Today;

            var query = _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .ThenInclude(c => c.Subject)
                .Where(a => a.AttendanceDate.Date == attendanceDate.Date);

            if (courseId.HasValue && courseId.Value > 0)
            {
                query = query.Where(a => a.CourseId == courseId.Value);
            }

            var attendances = await query
                .OrderBy(a => a.Course!.CourseName)
                .ThenBy(a => a.Student!.LastName)
                .ThenBy(a => a.Student!.FirstName)
                .ToListAsync();

            ViewBag.SelectedDate = attendanceDate;
            ViewBag.SelectedCourseId = courseId;
            ViewBag.Courses = new SelectList(await _context.Courses
                .Include(c => c.Subject)
                .Where(c => c.StartDate <= DateTime.Today && (c.EndDate == null || c.EndDate >= DateTime.Today))
                .OrderBy(c => c.CourseName)
                .ToListAsync(), "Id", "CourseName");

            return View(attendances);
        }

        // GET: Attendance/Take/5
        public async Task<IActionResult> Take(int? id, DateTime? date)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceDate = date ?? DateTime.Today;

            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Include(c => c.CourseEnrollments)
                .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            // Get existing attendance records for this date
            var existingAttendance = await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.CourseId == id && a.AttendanceDate.Date == attendanceDate.Date)
                .ToListAsync();

            // Create attendance records for students who don't have one yet
            var enrolledStudents = course.CourseEnrollments
                .Where(e => e.Status == "Enrolled")
                .Select(e => e.Student!)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToList();

            var attendanceList = new List<Attendance>();

            foreach (var student in enrolledStudents)
            {
                var existingRecord = existingAttendance.FirstOrDefault(a => a.StudentId == student.Id);
                if (existingRecord != null)
                {
                    attendanceList.Add(existingRecord);
                }
                else
                {
                    attendanceList.Add(new Attendance
                    {
                        StudentId = student.Id,
                        Student = student,
                        CourseId = course.Id,
                        Course = course,
                        AttendanceDate = attendanceDate,
                        Status = "Present"
                    });
                }
            }

            ViewBag.Course = course;
            ViewBag.AttendanceDate = attendanceDate;

            return View(attendanceList);
        }

        // POST: Attendance/Take
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Take(List<Attendance> attendances, int courseId, DateTime attendanceDate)
        {
            if (attendances == null || !attendances.Any())
            {
                TempData["Error"] = "No attendance data provided.";
                return RedirectToAction(nameof(Take), new { id = courseId, date = attendanceDate });
            }

            foreach (var attendance in attendances)
            {
                attendance.CourseId = courseId;
                attendance.AttendanceDate = attendanceDate;

                // Check if record already exists
                var existingRecord = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.StudentId == attendance.StudentId &&
                                           a.CourseId == courseId &&
                                           a.AttendanceDate.Date == attendanceDate.Date);

                if (existingRecord != null)
                {
                    // Update existing record
                    existingRecord.Status = attendance.Status;
                    existingRecord.Notes = attendance.Notes;
                    existingRecord.CheckInTime = attendance.CheckInTime;
                    _context.Update(existingRecord);
                }
                else
                {
                    // Create new record
                    _context.Add(attendance);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Attendance for {attendanceDate:yyyy-MM-dd} has been saved successfully.";

            return RedirectToAction(nameof(Index), new { date = attendanceDate, courseId = courseId });
        }

        // GET: Attendance/Student/5
        public async Task<IActionResult> Student(int? id, DateTime? startDate, DateTime? endDate)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;

            var attendances = await _context.Attendances
                .Include(a => a.Course)
                .ThenInclude(c => c!.Subject)
                .Where(a => a.StudentId == id &&
                           a.AttendanceDate >= start &&
                           a.AttendanceDate <= end)
                .OrderByDescending(a => a.AttendanceDate)
                .ThenBy(a => a.Course!.CourseName)
                .ToListAsync();

            ViewBag.Student = student;
            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.TotalDays = attendances.Count;
            ViewBag.PresentDays = attendances.Count(a => a.Status == "Present");
            ViewBag.LateDays = attendances.Count(a => a.Status == "Late");
            ViewBag.AbsentDays = attendances.Count(a => a.Status == "Absent");
            ViewBag.ExcusedDays = attendances.Count(a => a.Status == "Excused");
            ViewBag.AttendanceRate = attendances.Count > 0 ?
                Math.Round(((double)attendances.Count(a => a.IsPresent) / attendances.Count) * 100, 1) : 0;

            return View(attendances);
        }

        // GET: Attendance/Course/5
        public async Task<IActionResult> Course(int? id, DateTime? startDate, DateTime? endDate)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var start = startDate ?? DateTime.Today.AddDays(-30);
            var end = endDate ?? DateTime.Today;

            var attendances = await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.CourseId == id &&
                           a.AttendanceDate >= start &&
                           a.AttendanceDate <= end)
                .OrderByDescending(a => a.AttendanceDate)
                .ThenBy(a => a.Student!.LastName)
                .ThenBy(a => a.Student!.FirstName)
                .ToListAsync();

            var attendanceByDate = attendances
                .GroupBy(a => a.AttendanceDate.Date)
                .OrderByDescending(g => g.Key)
                .ToList();

            ViewBag.Course = course;
            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.TotalRecords = attendances.Count;
            ViewBag.TotalStudents = attendances.Select(a => a.StudentId).Distinct().Count();
            ViewBag.AttendanceByDate = attendanceByDate;

            return View(attendances);
        }

        // GET: Attendance/Report
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var start = startDate ?? DateTime.Today.AddDays(-7);
            var end = endDate ?? DateTime.Today;

            var attendanceSummary = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .ThenInclude(c => c!.Subject)
                .Where(a => a.AttendanceDate >= start && a.AttendanceDate <= end)
                .GroupBy(a => new { a.StudentId, a.Student!.FirstName, a.Student.LastName })
                .Select(g => new
                {
                    StudentId = g.Key.StudentId,
                    StudentName = g.Key.FirstName + " " + g.Key.LastName,
                    TotalDays = g.Count(),
                    PresentDays = g.Count(a => a.Status == "Present"),
                    LateDays = g.Count(a => a.Status == "Late"),
                    AbsentDays = g.Count(a => a.Status == "Absent"),
                    ExcusedDays = g.Count(a => a.Status == "Excused"),
                    AttendanceRate = g.Count() > 0 ? Math.Round(((double)g.Count(a => a.IsPresent) / g.Count()) * 100, 1) : 0
                })
                .OrderByDescending(x => x.AttendanceRate)
                .ToListAsync();

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.OverallAttendanceRate = attendanceSummary.Any() ?
                Math.Round(attendanceSummary.Average(s => s.AttendanceRate), 1) : 0;

            return View(attendanceSummary);
        }

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Attendance record updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { date = attendance.AttendanceDate });
            }

            // Reload navigation properties for view
            attendance.Student = await _context.Students.FindAsync(attendance.StudentId);
            attendance.Course = await _context.Courses
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == attendance.CourseId);

            return View(attendance);
        }

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Attendance record deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}