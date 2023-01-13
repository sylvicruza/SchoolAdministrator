using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public StudentAdministrationSystemContext dbContext;

        public HomeController(ILogger<HomeController> logger, StudentAdministrationSystemContext context)
        {
            _logger = logger;
            dbContext = context ;
        }

        public IActionResult Index()
        {
            ViewBag.programmeCount = dbContext.DegreeProgramme.ToList().Count;
            ViewBag.programmeTypeCount = dbContext.ProgrammeType.ToList().Count;
            ViewBag.studentCount = dbContext.Student.ToList().Count;
            ViewBag.courseCount = dbContext.CourseModule.ToList().Count;
            ViewBag.studentCourses = dbContext.Student.ToList().Count;
            ViewBag.assessmentCount = dbContext.Assessment.ToList().Count;
           
            return View();
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
}
