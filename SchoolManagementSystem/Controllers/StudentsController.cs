using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";

            var students = from s in _context.Students
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString)
                                       || s.StudentId.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "Id":
                    students = students.OrderBy(s => s.StudentId);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.StudentId);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            return View(await students.AsNoTracking().ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CourseEnrollments)
                    .ThenInclude(ce => ce.Course)
                        .ThenInclude(c => c!.Teacher)
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Course)
                .Include(s => s.Attendances)
                    .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,StudentId,DateOfBirth,PhoneNumber,Address")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    student.EnrollmentDate = DateTime.Now;
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CourseEnrollments)
                    .ThenInclude(ce => ce.Course)
                .Include(s => s.Grades)
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,StudentId,DateOfBirth,PhoneNumber,Address,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Student updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CourseEnrollments)
                    .ThenInclude(ce => ce.Course)
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Course)
                .Include(s => s.Attendances)
                    .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Enroll/5
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CourseEnrollments)
                    .ThenInclude(ce => ce.Course)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var enrolledCourseIds = student.CourseEnrollments.Select(ce => ce.CourseId).ToList();
            var availableCourses = await _context.Courses
                .Where(c => !enrolledCourseIds.Contains(c.Id) && (c.EndDate == null || c.EndDate > DateTime.Now))
                .Include(c => c.Teacher)
                .ToListAsync();

            ViewBag.Student = student;
            ViewBag.AvailableCourses = availableCourses;

            return View();
        }

        // POST: Students/Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollInCourse(int studentId, int courseId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var course = await _context.Courses.FindAsync(courseId);

            if (student == null || course == null)
            {
                return NotFound();
            }

            // Check if already enrolled
            var existingEnrollment = await _context.CourseEnrollments
                .FirstOrDefaultAsync(ce => ce.StudentId == studentId && ce.CourseId == courseId);

            if (existingEnrollment != null)
            {
                TempData["ErrorMessage"] = "Student is already enrolled in this course.";
                return RedirectToAction(nameof(Enroll), new { id = studentId });
            }

            var enrollment = new CourseEnrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Status = "Active"
            };

            _context.CourseEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Student successfully enrolled in {course.FullCourseName}!";
            return RedirectToAction(nameof(Details), new { id = studentId });
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}