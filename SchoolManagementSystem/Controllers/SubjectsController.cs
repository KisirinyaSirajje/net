using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolContext _context;

        public SubjectsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(EducationLevel? level, Models.Stream? stream)
        {
            var query = _context.Subjects.AsQueryable();

            if (level.HasValue)
            {
                query = query.Where(s => s.Level == level.Value);
            }

            if (stream.HasValue)
            {
                query = query.Where(s => s.Stream == stream.Value);
            }

            var subjects = await query
                .OrderBy(s => s.Level)
                .ThenBy(s => s.Stream)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewBag.SelectedLevel = level;
            ViewBag.SelectedStream = stream;

            return View(subjects);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Courses)
                .Include(s => s.StudentPerformances)
                .ThenInclude(sp => sp.Student)
                .Include(s => s.TeacherSubjects)
                .ThenInclude(ts => ts.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Level,Type,Stream,Description,IsActive,Credits")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.IsActive = true;
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Level,Type,Stream,Description,IsActive,Credits")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                // Instead of deleting, mark as inactive to preserve data integrity
                subject.IsActive = false;
                _context.Update(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Subjects/OLevel
        public async Task<IActionResult> OLevel()
        {
            var subjects = await _context.Subjects
                .Where(s => s.Level == EducationLevel.OLevel && s.IsActive)
                .OrderBy(s => s.Type)
                .ThenBy(s => s.Stream)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewBag.Title = "O-Level Subjects (S1-S4)";
            return View("LevelView", subjects);
        }

        // GET: Subjects/ALevel
        public async Task<IActionResult> ALevel()
        {
            var subjects = await _context.Subjects
                .Where(s => s.Level == EducationLevel.ALevel && s.IsActive)
                .OrderBy(s => s.Type)
                .ThenBy(s => s.Stream)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewBag.Title = "A-Level Subjects (S5-S6)";
            return View("LevelView", subjects);
        }

        // GET: Subjects/AssignTeacher/5
        public async Task<IActionResult> AssignTeacher(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .Where(t => t.IsActive)
                .OrderBy(t => t.FirstName)
                .ThenBy(t => t.LastName)
                .ToListAsync();

            var assignedTeachers = await _context.TeacherSubjects
                .Where(ts => ts.SubjectId == id && ts.IsActive)
                .Include(ts => ts.Teacher)
                .ToListAsync();

            ViewBag.Subject = subject;
            ViewBag.Teachers = teachers;
            ViewBag.AssignedTeachers = assignedTeachers;

            return View();
        }

        // POST: Subjects/AssignTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeacher(int subjectId, int teacherId, QualificationLevel qualificationLevel)
        {
            // Check if assignment already exists
            var existingAssignment = await _context.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.SubjectId == subjectId && ts.TeacherId == teacherId && ts.IsActive);

            if (existingAssignment != null)
            {
                TempData["Error"] = "Teacher is already assigned to this subject.";
                return RedirectToAction(nameof(AssignTeacher), new { id = subjectId });
            }

            var teacherSubject = new TeacherSubject
            {
                SubjectId = subjectId,
                TeacherId = teacherId,
                QualificationLevel = qualificationLevel,
                AssignedDate = DateTime.Now,
                IsActive = true,
                IsPrimarySubject = false,
                YearsOfExperience = 0
            };

            _context.Add(teacherSubject);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Teacher assigned successfully.";
            return RedirectToAction(nameof(AssignTeacher), new { id = subjectId });
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}