using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolContext _context;

        public TeachersController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DeptSortParm"] = sortOrder == "Dept" ? "dept_desc" : "Dept";
            ViewData["HireSortParm"] = sortOrder == "Hire" ? "hire_desc" : "Hire";

            var teachers = from t in _context.Teachers.Include(t => t.Courses)
                          select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t => t.LastName.Contains(searchString)
                                       || t.FirstName.Contains(searchString)
                                       || t.EmployeeId.Contains(searchString)
                                       || t.Email.Contains(searchString)
                                       || (t.Department != null && t.Department.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    teachers = teachers.OrderByDescending(t => t.LastName);
                    break;
                case "Dept":
                    teachers = teachers.OrderBy(t => t.Department);
                    break;
                case "dept_desc":
                    teachers = teachers.OrderByDescending(t => t.Department);
                    break;
                case "Hire":
                    teachers = teachers.OrderBy(t => t.HireDate);
                    break;
                case "hire_desc":
                    teachers = teachers.OrderByDescending(t => t.HireDate);
                    break;
                default:
                    teachers = teachers.OrderBy(t => t.LastName);
                    break;
            }

            return View(await teachers.AsNoTracking().ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                    .ThenInclude(c => c.CourseEnrollments)
                        .ThenInclude(ce => ce.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,EmployeeId,PhoneNumber,Department,Specialization,Address,YearsOfExperience")] Teacher teacher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teacher.HireDate = DateTime.Now;
                    _context.Add(teacher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,EmployeeId,PhoneNumber,Department,Specialization,HireDate,Address,YearsOfExperience")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Teacher updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.Include(t => t.Courses).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher != null)
            {
                if (teacher.Courses.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete teacher. Teacher has assigned courses.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
                
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Teacher deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}