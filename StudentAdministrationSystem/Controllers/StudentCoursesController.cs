using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ObjectBuilder2;
using MySqlX.XDevAPI.Common;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.DTO;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Services;
using static StudentAdministrationSystem.DTO.AssessmentDTO;

namespace StudentAdministrationSystem.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class StudentCoursesController : Controller
    {
        private readonly StudentAdministrationSystemContext _context;

        public StudentCoursesController(StudentAdministrationSystemContext context)
        {
            _context = context;
        }

        // GET: Enrollment
        public async Task<IActionResult> Index()
        {
            var studentAdministrationSystemContext = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student);
            return View(await studentAdministrationSystemContext.ToListAsync());
        }

        public async Task<IActionResult> StudentCourse(string response)
        {

            ViewBag.Message = response;
            var studentAdministrationSystemContext = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student);
            ViewData["AllStudents"] = _context.Student.Include(s => s.DegreeProgramme.CourseModules);
            return View(await studentAdministrationSystemContext.ToListAsync());
        }




        [HttpGet("{id}/{courseId}")]
        public IActionResult StudentView(int? id, int? courseId)
        {

            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student).Include(s => s.Assessment).Where(a => a.StudentId == id).Where(a => a.CourseModuleId == courseId);

            ViewBag.student = _context.Student.Include(s => s.DegreeProgramme).FirstOrDefault(a => a.Id == id);
            return View(studentCourses);
        }

        public async Task<IActionResult> GradeStudent(string response)
        {

            ViewBag.Message = response;
            GradeStudent gradeStudent = new GradeStudent(_context);

            ViewData["AllStudents"] = gradeStudent.GetAllStudentResults();
            return View();
        }

        public async Task<IActionResult> GradeStudentProgramme(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = _context.Enrollment
                .Include(s => s.CourseModule)
                .Include(s => s.DegreeProgramme)
                .Include(s => s.Student)
                .Include(s => s.Assessment)
                .Where(a => a.StudentId == id);


            GradeStudent GradeStudent = new GradeStudent(_context);
            var studentGrade = GradeStudent.FinalResult((int)id);


            ViewBag.student = studentGrade;
            return View(studentCourses);
        }

        // GET: Enrollment/GradeStudentCreate
        public IActionResult GradeStudentCreate(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            //This code section display the list of modules enrolled and student details.
            var assigned = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student).Include(a => a.Assessment).Where(a => a.StudentId == id);
            ViewBag.assigned = assigned;

            Enrollment studentC = _context.Enrollment.Include(a => a.Assessment).FirstOrDefaultAsync(a => a.Id == id).Result;
            var studentId = studentC.StudentId;
            var student = _context.Student.Include(s => s.DegreeProgramme.CourseModules).FirstOrDefaultAsync(m => m.Id == studentId).Result;

            ViewData["CourseModuleId"] = studentC.CourseModule.CourseTitle;
            ViewData["DegreeProgrammeId"] = student.DegreeProgramme.Title;
            ViewData["AssessmentId"] = studentC.Assessment.AssessmentName + "            (" + studentC.Assessment.Marks + ") " + "Marks";
            ViewData["StudentId"] = "(" + student.StudentNumber + ")" + "   " + student.FirstName + " " + student.LastName;
            var assessmentDTO = new AssessmentDTO();
            assessmentDTO.Title = studentC.CourseModule.CourseTitle;


            //Calculates the assessment score for a student
            var assessList = new List<AssessmentDTO.AssessmentInfo>();
            foreach (var ls in _context.Enrollment.Include(a => a.Assessment).ToList().Where(a => a.CourseModuleId == studentC.CourseModuleId).Where(a => a.StudentId == studentC.StudentId))
            {
                AssessmentDTO.AssessmentInfo info = new AssessmentDTO.AssessmentInfo();
                info.AssessmentName = ls.Assessment.AssessmentName;
                info.Marks = (int)ls.Mark;
                assessList.Add(info);
            }
            assessmentDTO.Info = assessList;
            assessmentDTO.Overall = assessList.Sum(i => i.Marks);
            ViewBag.assessmentScore = assessmentDTO;

            return View(studentC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeStudentEdit(int id, [Bind("Id,StudentId,DegreeProgrammeId,CourseModuleId,Mark", "AssessmentId")] Enrollment studentCourses)
        {
            if (id != studentCourses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        _context.Enrollment.Update(studentCourses);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StudentCoursesExists(studentCourses.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    var message = "Student score added successfully";
                    return RedirectToAction(nameof(GradeStudent), new { response = message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.Failure = "An error occur while performing request";

                    return View(studentCourses);
                }
            }
            ViewBag.Failure = "Failed to assign grade";

            return View(studentCourses);
        }

        // GET: Enrollment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = _context.Enrollment.Where(m => m.StudentId == id)
                .Include(s => s.CourseModule)
                .Include(s => s.DegreeProgramme)
                .Include(s => s.Student)
                .Include(s => s.Assessment);

            if (studentCourses == null)
            {
                return NotFound();
            }
            AssessmentDTO courses = new AssessmentDTO();
            courses.Student = studentCourses.First().Student;
            courses.DegreeProgramme = studentCourses.First().DegreeProgramme;


            List<AssessmentDTO.CourseInfo> courseInfos = new List<AssessmentDTO.CourseInfo>();
            List<AssessmentDTO.AssessmentInfo> assessList = new List<AssessmentDTO.AssessmentInfo>();


            foreach (var ls in studentCourses)
            {
                AssessmentDTO.CourseInfo courseInfo = new AssessmentDTO.CourseInfo();
                AssessmentDTO.AssessmentInfo info = new AssessmentDTO.AssessmentInfo();
                info.AssessmentName = ls.Assessment.AssessmentName;
                info.Marks = (int)ls.Mark;
                info.Id = ls.Id;
                info.CourseName = ls.Assessment.Course.CourseTitle;
                assessList.Add(info);

                courseInfo.Code = ls.CourseModule.CourseCode;
                courseInfo.Marks = ls.CourseModule.Marks;
                courseInfo.Name = ls.CourseModule.CourseTitle;
                courseInfo.Type = ls.CourseModule.CourseType;
                courseInfo.Id = ls.CourseModuleId;
                courseInfos.Add(courseInfo);
            }

            courses.Info = assessList;
            HashSet<CourseInfo> myHashset = new HashSet<CourseInfo>(courseInfos);
            var courseModulesAssigned = myHashset.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            courses.Course = courseModulesAssigned;




            ViewBag.StudentCourses = courses;

            return View();
        }

        //Assigned course to student
        // GET: Enrollment/Create
        public IActionResult Create(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            //Get List of assigned courses to students
            var enrollments = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student).Include(s => s.Assessment).Where(a => a.StudentId == id);
            HashSet<Enrollment> myHashset = new HashSet<Enrollment>(enrollments);
            var courseModulesAssigned = myHashset.GroupBy(x => x.CourseModuleId).Select(x => x.First()).ToList();


            ViewBag.assigned = courseModulesAssigned;

            var student = _context.Student
                .Include(s => s.DegreeProgramme.CourseModules)
                .FirstOrDefaultAsync(m => m.Id == id);
            var courses = student.Result.DegreeProgramme.CourseModules.ToList();





            ViewData["CourseModuleId"] = courses;
            ViewData["DegreeProgrammeId"] = student.Result.DegreeProgramme.Title;
            ViewData["StudentId"] = "(" + student.Result.StudentNumber + ")" + "   " + student.Result.FirstName + " " + student.Result.LastName;
            var studentCourses = new Enrollment();
            studentCourses.DegreeProgrammeId = student.Result.DegreeProgramme.ID;
            studentCourses.StudentId = student.Result.Id;

            return View(studentCourses);
        }

        //Assigned course to student
        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,DegreeProgrammeId,CourseModuleId")] Enrollment studentCourses)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Assign assessment to 
                    var assessments = _context.Assessment.ToList().Where(a => a.CourseModuleId == studentCourses.CourseModuleId);
                    foreach (var studentAssessment in assessments)
                    {
                        if (assessments.Count() > 0)
                        {
                            var myassessment = new Enrollment
                            {
                                DegreeProgrammeId = studentCourses.DegreeProgrammeId,
                                StudentId = studentCourses.StudentId,
                                CourseModuleId = studentCourses.CourseModuleId,
                                AssessmentId = studentAssessment.AssessmentID,
                                Mark = 0
                            };
                            _context.Enrollment.Add(myassessment);
                        }
                    }

                    await _context.SaveChangesAsync();
                    var message = "Student assigned to a course successfully";
                    return RedirectToAction(nameof(StudentCourse), new { response = message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.FailMessage = e.Message;
                  
                }
            }
            ViewBag.Failure = "Failed to assign course";
            ViewData["CourseModuleId"] = _context.CourseModule.ToListAsync().Result;
            ViewData["DegreeProgrammeId"] = _context.DegreeProgramme.ToListAsync().Result;
            ViewData["StudentId"] = _context.Student.ToListAsync().Result;
            return View(studentCourses);
        }

        // GET: Enrollment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = await _context.Enrollment.FindAsync(id);
            if (studentCourses == null)
            {
                return NotFound();
            }
            ViewData["CourseModuleId"] = new SelectList(_context.CourseModule, "ID", "CourseTitle", studentCourses.CourseModuleId);
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", studentCourses.DegreeProgrammeId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "StudentNumber", studentCourses.StudentId);
            return View(studentCourses);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,DegreeProgrammeId,CourseModuleId,Mark")] Enrollment studentCourses)
        {
            if (id != studentCourses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentCourses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentCoursesExists(studentCourses.Id))
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
            ViewData["CourseModuleId"] = new SelectList(_context.CourseModule, "ID", "Title", studentCourses.CourseModuleId);
            ViewData["DegreeProgrammeId"] = new SelectList(_context.DegreeProgramme, "ID", "Title", studentCourses.DegreeProgrammeId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Email", studentCourses.StudentId);
            return View(studentCourses);
        }

        // GET: Enrollment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = await _context.Enrollment
                .Include(s => s.CourseModule)
                .Include(s => s.DegreeProgramme)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentCourses == null)
            {
                return NotFound();
            }

            return View(studentCourses);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentCourses = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(studentCourses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentCoursesExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult GetStudentInfoByStudentId(int? studentId)
        {
            var studentAdministrationSystemContext = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student);

            var studentList = studentAdministrationSystemContext.Where(a => a.Id == studentId).ToList();
            return Json(studentList);
        }

        [HttpPost]
        public JsonResult GetCoursesByDegreeProgrammeId(int departmentId)
        {

            var studentAdministrationSystemContext = _context.Enrollment.Include(s => s.CourseModule).Include(s => s.DegreeProgramme).Include(s => s.Student);

            var coursesList = studentAdministrationSystemContext.Where(a => a.DegreeProgramme.ID == departmentId).ToList();
            return Json(coursesList);
        }



        public async Task<IActionResult> ResultView()
        {

            GradeStudent result = new GradeStudent(_context);
            var studentResult = result.GetAllStudentResults();


            ViewBag.student = studentResult;
            ViewData["AllStudents"] = _context.Student.Include(s => s.DegreeProgramme.CourseModules);
            return View();
        }

        public IActionResult StudentReport(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var studentCourses = _context.Enrollment
                .Include(s => s.CourseModule)
                .Include(s => s.DegreeProgramme)
                .Include(s => s.Student)
                .Include(s => s.Assessment)
                .Where(a => a.StudentId == id);


            GradeStudent GradeStudent = new GradeStudent(_context);
            var studentGrade = GradeStudent.FinalResult((int)id);


            ViewBag.student = studentGrade;
            return View(studentCourses);
        }

        public IActionResult PrintReport(int id)
        {

            GenerateReport report = new GenerateReport(_context);
            var file = report.CreateResultInPDF(id);
            return File(file, "application/pdf");


        }

        public bool IsModuleAlreadyExists(int courseId, int studentId)
        {

            Console.WriteLine("Course<><><><>", courseId);
            var enrollments = _context.Enrollment.ToList().Where(c => c.CourseModuleId == courseId).Where(c => c.StudentId == studentId);
            var counter = enrollments.Count();
            if (counter > 0)
            {
                return true;
            }


            return false;
        }

        [HttpPost, ActionName("GetAssessments")]
        public IActionResult GetAssessments([FromBody] int CourseModuleId)
        {

            var assessments = _context.Assessment.ToList().Where(a => a.CourseModuleId == CourseModuleId);
            return Json(assessments);



        }



    }
}
