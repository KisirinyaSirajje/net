using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    public class GradingController : Controller
    {
        private readonly SchoolContext _context;

        public GradingController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Grading
        public async Task<IActionResult> Index()
        {
            var currentYear = DateTime.Now.Year.ToString();
            var students = await _context.Students
                .Where(s => s.IsActive)
                .Include(s => s.CourseEnrollments)
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();

            ViewBag.CurrentYear = currentYear;
            return View(students);
        }

        // GET: Grading/Student/5
        public async Task<IActionResult> Student(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.CourseEnrollments)
                .ThenInclude(e => e.Course)
                .ThenInclude(c => c.Subject)
                .Include(s => s.SubjectPerformances)
                .ThenInclude(sp => sp.Subject)
                .Include(s => s.SubjectPerformances)
                .ThenInclude(sp => sp.GradeScale)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.Subjects = await _context.Subjects
                .Where(s => s.IsActive && s.Level == student.CurrentLevel)
                .ToListAsync();

            ViewBag.GradeScales = await _context.GradeScales
                .Where(g => g.Level == student.CurrentLevel)
                .OrderBy(g => g.DisplayOrder)
                .ToListAsync();

            return View(student);
        }

        // POST: Grading/AddGrade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(StudentSubjectPerformance performance)
        {
            if (ModelState.IsValid)
            {
                // Set default values
                performance.SubmittedDate = DateTime.Now;
                performance.LastModified = DateTime.Now;
                performance.ResultStatus = ResultStatus.Submitted;

                // Calculate grade based on score
                if (performance.Score.HasValue)
                {
                    var student = await _context.Students.FindAsync(performance.StudentId);
                    var gradeScale = await _context.GradeScales
                        .FirstOrDefaultAsync(g => 
                            g.Level == student.CurrentLevel &&
                            performance.Score >= g.MinMark && 
                            performance.Score <= g.MaxMark);

                    if (gradeScale != null)
                    {
                        performance.GradeScaleId = gradeScale.Id;
                        performance.LetterGrade = gradeScale.Grade;
                        performance.GradePoint = gradeScale.GradePoint;
                    }
                }

                _context.Add(performance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Student), new { id = performance.StudentId });
            }

            return RedirectToAction(nameof(Student), new { id = performance.StudentId });
        }

        // GET: Grading/ReportCard/5
        public async Task<IActionResult> ReportCard(int? id, string? academicYear, Term? term)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentYear = academicYear ?? DateTime.Now.Year.ToString();
            var currentTerm = term ?? Term.Term1;

            var student = await _context.Students
                .Include(s => s.SubjectPerformances.Where(sp => 
                    sp.AcademicYear == currentYear && sp.Term == currentTerm))
                .ThenInclude(sp => sp.Subject)
                .Include(s => s.SubjectPerformances)
                .ThenInclude(sp => sp.GradeScale)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            // Calculate overall performance statistics
            var performances = student.SubjectPerformances
                .Where(sp => sp.AcademicYear == currentYear && sp.Term == currentTerm)
                .ToList();

            ViewBag.TotalSubjects = performances.Count;
            ViewBag.PassedSubjects = performances.Count(p => p.GradeScale?.IsPassingGrade == true);
            ViewBag.AverageScore = performances.Where(p => p.Score.HasValue).Average(p => p.Score);
            ViewBag.AverageGradePoint = performances.Where(p => p.GradePoint.HasValue).Average(p => p.GradePoint);
            ViewBag.CurrentYear = currentYear;
            ViewBag.CurrentTerm = currentTerm;

            return View(student);
        }

        // GET: Grading/ClassReport
        public async Task<IActionResult> ClassReport(EducationLevel? level, Models.Stream? stream, string? academicYear, Term? term)
        {
            var currentYear = academicYear ?? DateTime.Now.Year.ToString();
            var currentTerm = term ?? Term.Term1;
            var selectedLevel = level ?? EducationLevel.OLevel;
            var selectedStream = stream ?? Models.Stream.NotApplicable;

            var students = await _context.Students
                .Where(s => s.IsActive && s.CurrentLevel == selectedLevel)
                .Where(s => selectedStream == Models.Stream.NotApplicable || s.Stream == selectedStream)
                .Include(s => s.SubjectPerformances.Where(sp => 
                    sp.AcademicYear == currentYear && sp.Term == currentTerm))
                .ThenInclude(sp => sp.Subject)
                .Include(s => s.SubjectPerformances)
                .ThenInclude(sp => sp.GradeScale)
                .OrderBy(s => s.FirstName)
                .ThenBy(s => s.LastName)
                .ToListAsync();

            ViewBag.CurrentYear = currentYear;
            ViewBag.CurrentTerm = currentTerm;
            ViewBag.SelectedLevel = selectedLevel;
            ViewBag.SelectedStream = selectedStream;

            return View(students);
        }

        // GET: Grading/SubjectReport/5
        public async Task<IActionResult> SubjectReport(int? id, string? academicYear, Term? term)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentYear = academicYear ?? DateTime.Now.Year.ToString();
            var currentTerm = term ?? Term.Term1;

            var subject = await _context.Subjects
                .Include(s => s.StudentPerformances.Where(sp => 
                    sp.AcademicYear == currentYear && sp.Term == currentTerm))
                .ThenInclude(sp => sp.Student)
                .Include(s => s.StudentPerformances)
                .ThenInclude(sp => sp.GradeScale)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
            {
                return NotFound();
            }

            var performances = subject.StudentPerformances
                .Where(sp => sp.AcademicYear == currentYear && sp.Term == currentTerm)
                .ToList();

            ViewBag.TotalStudents = performances.Count;
            ViewBag.PassedStudents = performances.Count(p => p.GradeScale?.IsPassingGrade == true);
            ViewBag.AverageScore = performances.Where(p => p.Score.HasValue).Average(p => p.Score);
            ViewBag.HighestScore = performances.Where(p => p.Score.HasValue).Max(p => p.Score);
            ViewBag.LowestScore = performances.Where(p => p.Score.HasValue).Min(p => p.Score);
            ViewBag.CurrentYear = currentYear;
            ViewBag.CurrentTerm = currentTerm;

            return View(subject);
        }

        // GET: Grading/EditGrade/5
        public async Task<IActionResult> EditGrade(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.StudentSubjectPerformances
                .Include(p => p.Student)
                .Include(p => p.Subject)
                .Include(p => p.GradeScale)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (performance == null)
            {
                return NotFound();
            }

        var gradeScales = await _context.GradeScales
            .Where(g => g.Level == performance.Student.CurrentLevel)
            .OrderBy(g => g.DisplayOrder)
            .ToListAsync();

        ViewBag.GradeScales = gradeScales;
        ViewBag.GradeScalesJson = gradeScales.Select(g => new { 
            grade = g.Grade, 
            minMark = g.MinMark, 
            maxMark = g.MaxMark, 
            description = g.Description, 
            gradePoint = g.GradePoint, 
            isPassingGrade = g.IsPassingGrade 
        });            return View(performance);
        }

        // POST: Grading/EditGrade/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(int id, StudentSubjectPerformance performance)
        {
            if (id != performance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    performance.LastModified = DateTime.Now;

                    // Recalculate grade based on score
                    if (performance.Score.HasValue)
                    {
                        var student = await _context.Students.FindAsync(performance.StudentId);
                        var gradeScale = await _context.GradeScales
                            .FirstOrDefaultAsync(g => 
                                g.Level == student.CurrentLevel &&
                                performance.Score >= g.MinMark && 
                                performance.Score <= g.MaxMark);

                        if (gradeScale != null)
                        {
                            performance.GradeScaleId = gradeScale.Id;
                            performance.LetterGrade = gradeScale.Grade;
                            performance.GradePoint = gradeScale.GradePoint;
                        }
                    }

                    _context.Update(performance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentSubjectPerformanceExists(performance.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Student), new { id = performance.StudentId });
            }

            var studentForGrades = await _context.Students.FindAsync(performance.StudentId);
            var gradeScalesForPost = await _context.GradeScales
                .Where(g => g.Level == studentForGrades.CurrentLevel)
                .OrderBy(g => g.DisplayOrder)
                .ToListAsync();

            ViewBag.GradeScales = gradeScalesForPost;
            ViewBag.GradeScalesJson = gradeScalesForPost.Select(g => new { 
                grade = g.Grade, 
                minMark = g.MinMark, 
                maxMark = g.MaxMark, 
                description = g.Description, 
                gradePoint = g.GradePoint, 
                isPassingGrade = g.IsPassingGrade 
            });

            return View(performance);
        }

        private bool StudentSubjectPerformanceExists(int id)
        {
            return _context.StudentSubjectPerformances.Any(e => e.Id == id);
        }
    }
}