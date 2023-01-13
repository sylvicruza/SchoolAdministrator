using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Services;


namespace StudentAdministrationSystem.Controllers
{
    public class CourseModulesController : Controller
    {
        public ICourseService CourseService;
        public StudentAdministrationSystemContext dbContext;
        public CourseModulesController(ICourseService _CourseService, StudentAdministrationSystemContext dbContext)
        {
            CourseService = _CourseService;
            this.dbContext = dbContext; 
        }


        // GET: CourseModules
        public async Task<IActionResult> Index(string response)
        {
            ViewBag.Message = response;
            return View(await CourseService.GetAllCourses());
        }

        // GET: CourseModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseModule = await CourseService.GetCourseFirstById(id);
            if (courseModule == null)
            {
                return NotFound();
            }

            return View(courseModule);
        }

        // GET: CourseModules/Create
        public IActionResult Create()
        {
            ViewBag.Programme = dbContext.DegreeProgramme.ToList();
            
            return View(new CourseModule());
        }

        // POST: CourseModules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CourseTitle,Title,CourseCode,CourseType,DegreeProgrammeId,Marks")] CourseModule courseModule)
        {
   
   
            if (ModelState.IsValid)
            {
                try
                {
                    var courseObj = new CourseModule();
                    courseObj.CourseTitle = courseModule.CourseTitle;
                    courseObj.CourseCode = courseModule.CourseCode;
                    courseObj.CourseType = courseModule.CourseType;
                    courseObj.Marks = courseModule.Marks;
                    courseObj.DegreeProgrammeId = courseModule.DegreeProgrammeId;
                    await CourseService.CreateCourse(courseObj);
                    var message = "Course created successfully";
                    return RedirectToAction("Index", "CourseModules", new { response = message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.Failure = "An error occurred while processing your request";
                    ViewBag.Programme = dbContext.DegreeProgramme.ToList();
                    return View(courseModule);
                }
            }
            ViewBag.Failure = "An error occurred while processing your request";
            ViewBag.Programme = dbContext.DegreeProgramme.ToList();
            return View(courseModule);
        }

        // GET: CourseModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseModule = await CourseService.GetCourseByCourseId(id);
            if (courseModule == null)
            {
                return NotFound();
            }
            ViewBag.Programme = dbContext.DegreeProgramme.ToList();
            return View(courseModule);
        }

        // POST: CourseModules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CourseTitle,Title,CourseType,DegreeProgrammeId,Marks")] CourseModule courseModule)
        {
            if (id != courseModule.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await CourseService.UpdateCourse(courseModule);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseModuleExists(courseModule.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var message = "Course updated successfully";
                return RedirectToAction("Index", "CourseModules", new { response = message });
            }
            ViewBag.Failure = "Failed to update course";
            return View(courseModule);
        }

        // GET: CourseModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseModule = await CourseService.GetCourseFirstById(id);
            if (courseModule == null)
            {
                return NotFound();
            }

            return View(courseModule);
        }

        // POST: CourseModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseModule = await CourseService.GetCourseFirstById(id);
            await CourseService.DeleteCourse(courseModule);
            return RedirectToAction(nameof(Index));
        }

        private bool CourseModuleExists(int id)
        {
            return CourseService.GetCourseIdExist(id);
        }

        public JsonResult IsCourseTitleExists(string title)
        {
            List<CourseModule> courses =  CourseService.GetAllCourses().Result.Where(a => a.CourseTitle == title).ToList();
            if (courses.Count > 0) { 
                return Json(false);
            }
            return Json(true);
        }

        public JsonResult IsCourseCodeExists(string code)
        {
            List<CourseModule> courses = CourseService.GetAllCourses().Result.Where(a => a.CourseCode == code).ToList();
            if (courses.Count > 0) { return Json(false); }
            return Json(true);
        }
    }
}
