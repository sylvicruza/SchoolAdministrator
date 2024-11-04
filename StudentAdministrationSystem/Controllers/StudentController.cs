using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.DTO;
using StudentAdministrationSystem.Models;

namespace StudentAdministrationSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentAdministrationSystemContext _context;

        public StudentController(StudentAdministrationSystemContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(string response)
        {
            ViewBag.Message = response;
            GradeStudent result = new GradeStudent(_context);
            ViewBag.student = result.GetAllStudentResults();

            return View();
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.DegreeProgramme.CourseModules)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            var courseList = _context.CourseModule.ToList().Where(a => a.DegreeProgrammeId == student.DegreeProgrammeId);

            //Get List of assigned courses to students
            var Result = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student).Include(s => s.Assessment).Where(a => a.StudentId == student.Id);
           
            HashSet<Enrollment> myHashset = new HashSet<Enrollment>(Result);
            var distinctList = myHashset.GroupBy(x => x.CourseModuleId).Select(x => x.First()).ToList();
            ViewBag.assigned = distinctList;
            ViewBag.assignedcount= distinctList.Count();
            ViewBag.availablecount = courseList.Count();
            var assessments = _context.Assessment.ToList().Where(a => a.CourseModuleId == id);


            ViewBag.available = courseList;

            return View(student);
        }



        // GET: Student/Create
        public IActionResult Create()
        {
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title");
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,ContactNo,Address,DegreeProgrammeId")] Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    var count = await _context.Student.CountAsync();
                    var Today = DateTime.Now;
                    student.EnrollmentDate = Today;
                    student.Cohort = Today.Year.ToString();

                    //This code generate student Number by Id
                    switch (count)
                    {
                        case 0:
                            student.StudentNumber = Today.Year.ToString() + 1.ToString("D6");

                            break;
                        default:                      
                            int lastColumn = _context.Student.OrderBy(x => x.Id).LastOrDefault().Id;
                            lastColumn++;
                            student.StudentNumber = Today.Year.ToString() + lastColumn.ToString("D6");

                            break;
                    }
                    _context.Add(student);
                    await _context.SaveChangesAsync();

                    //This code Assign Mandatory courses and assignment by default
                    var courseList = _context.CourseModule.ToList().Where(c => c.DegreeProgrammeId == student.DegreeProgrammeId);
                    foreach (var studentCourses in from course in courseList
                                                   where course.CourseType == "Mandatory"
                                                   let assessments = _context.Assessment.ToList().Where(a => a.CourseModuleId == course.ID)
                                                   from assessment in assessments
                                                   let studentCourses = new Enrollment
                                                   {
                                                       DegreeProgrammeId = course.DegreeProgrammeId,
                                                       StudentId = student.Id,
                                                       CourseModuleId = course.ID,
                                                       AssessmentId = assessment.AssessmentID,
                                                       Mark = 0
                                                   }
                                                   select studentCourses)
                    {
                        _context.Enrollment.Add(studentCourses);
                    }

                    await _context.SaveChangesAsync();
                    var message = "Student with Id: " + student.StudentNumber + " has been created successfully ";
                    return RedirectToAction("Index", "Student", new { response = message });
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.Failure = "An error occurred while processing your request.";
                    ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", student.DegreeProgrammeId);
                    return View(student);
                }
            }
            ViewBag.Failure = "An error occurred while processing your request.";
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", student.DegreeProgrammeId);
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", student.DegreeProgrammeId);
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,ContactNo,Address,DegreeProgrammeId")] Student student)
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
                var message = "Student with Id: " + student.StudentNumber + "has been updated successfully ";
                return RedirectToAction("Index", "Student", new { response = message });
            }
            ViewBag.Failure = "Failed to update Student";
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", student.DegreeProgrammeId);
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.DegreeProgramme)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
        public JsonResult IsStudentEmailExists(string email)
        {
            List<Student> students = _context.Student.Where(a => a.Email == email).ToList();
            if (students.Count > 0) { return Json(false); }
            return Json(true);
        }
    }
}
