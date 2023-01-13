using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Services;
using StudentAdministrationSystem.Services.Implementation;

namespace StudentAdministrationSystem.Controllers
{
    public class AssessmentsController : Controller
    {
        private readonly IAssessmentService IAssessmentService;
       

        public AssessmentsController(IAssessmentService _IAssessmentService)
        {
            IAssessmentService = _IAssessmentService;
            
        }

        // GET: Assessments
        public async Task<IActionResult> Index(string response)
        {
            ViewBag.Message = response;
            return View(await IAssessmentService.GetAllAssessments());
        }

        // GET: Assessments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessment = await IAssessmentService.GetAssessmentFirstById(id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // GET: Assessments/Create
        public IActionResult Create()
        {
            ViewBag.AllCourses = IAssessmentService.GetAllCourses();

            return View();
        }

        // POST: Assessments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssessmentID,AssessmentName,Marks,CourseModuleId")] Assessment assessment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await IAssessmentService.CreateAssessment(assessment);
                    var message = "Assessment created successfully";
                    return RedirectToAction("Index", "Assessments", new { response = message });
                }
                catch (Exception e)
                {
                   Console.WriteLine(e.Message);
                    ViewBag.Failure = "An error occurred while processing your request.";
                    return View(assessment);
                }
            }
            ViewBag.Failure = "An error occurred while processing your request.";
            return View(assessment);
        }

        // GET: Assessments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessment = await IAssessmentService.GetAssessmentByAssessmentId(id);
            if (assessment == null)
            {
                return NotFound();
            }
            ViewBag.AllCourses = IAssessmentService.GetAllCourses();
            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssessmentID,AssessmentName,Marks,Course")] Assessment assessment)
        {
            if (id != assessment.AssessmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    await IAssessmentService.UpdateAssessment(assessment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentExists(assessment.AssessmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var message = "Assessment updated successfully";
                return RedirectToAction("Index", "Assessments", new { response = message });

            }
            ViewBag.Failure = "Failed to update Assessment";
            return View(assessment);
        }

        // GET: Assessments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessment = await IAssessmentService.GetAssessmentFirstById(id);
            if (assessment == null)
            {
                return NotFound();
            }

            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assessment = await IAssessmentService.GetAssessmentByAssessmentId(id);
            await IAssessmentService.DeleteAssessment(assessment);
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentExists(int id)
        {
            return IAssessmentService.GetAssessmentIdExist(id);
        }

        public JsonResult IsAssessmentsMarkExists(float marks)
        {
            return Json(true);
        }
    }
}
