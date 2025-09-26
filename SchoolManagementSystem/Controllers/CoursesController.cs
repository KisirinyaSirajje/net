using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "code_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["TeacherSortParm"] = sortOrder == "Teacher" ? "teacher_desc" : "Teacher";
            ViewData["CreditsSortParm"] = sortOrder == "Credits" ? "credits_desc" : "Credits";

            var courses = from c in _context.Courses
                         .Include(c => c.Teacher)
                         .Include(c => c.CourseEnrollments)
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.CourseCode.Contains(searchString)
                                       || c.CourseName.Contains(searchString)
                                       || (c.Teacher != null && (c.Teacher.FirstName.Contains(searchString) || c.Teacher.LastName.Contains(searchString))));
            }

            switch (sortOrder)
            {
                case "code_desc":
                    courses = courses.OrderByDescending(c => c.CourseCode);
                    break;
                case "Name":
                    courses = courses.OrderBy(c => c.CourseName);
                    break;
                case "name_desc":
                    courses = courses.OrderByDescending(c => c.CourseName);
                    break;
                case "Teacher":
                    courses = courses.OrderBy(c => c.Teacher!.LastName);
                    break;
                case "teacher_desc":
                    courses = courses.OrderByDescending(c => c.Teacher!.LastName);
                    break;
                case "Credits":
                    courses = courses.OrderBy(c => c.Credits);
                    break;
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                default:
                    courses = courses.OrderBy(c => c.CourseCode);
                    break;
            }

            return View(await courses.AsNoTracking().ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.CourseEnrollments)
                    .ThenInclude(ce => ce.Student)
                .Include(c => c.Grades)
                    .ThenInclude(g => g.Student)
                .Include(c => c.Attendances)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,CourseName,Description,Credits,TeacherId,StartDate,EndDate,MaxEnrollment,DaysOfWeek,StartTime,EndTime,Room")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", course.TeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseCode,CourseName,Description,Credits,TeacherId,StartDate,EndDate,MaxEnrollment,DaysOfWeek,StartTime,EndTime,Room")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Course updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "FullName", course.TeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.CourseEnrollments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CourseEnrollments)
                .Include(c => c.Grades)
                .Include(c => c.Attendances)
                .FirstOrDefaultAsync(c => c.Id == id);
                
            if (course != null)
            {
                if (course.CourseEnrollments.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete course. Students are enrolled in this course.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
                
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Course deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}