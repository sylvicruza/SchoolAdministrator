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
    public class ProgrammeController : Controller
    {
        private readonly IProgrammeService programmeService;
        private readonly IProgrammeTypeService programmeTypeService;

        public ProgrammeController(IProgrammeService ProgrammeService, IProgrammeTypeService programmeTypeService)
        {
            this.programmeService = ProgrammeService;
            this.programmeTypeService = programmeTypeService;
        }

        // GET: Programme
        public async Task<IActionResult> Index(string response)
        {
            ViewBag.Message = response;
            return View(await programmeService.FindDegreeProgrammeAll());
        }

        // GET: Programme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degreeProgramme = await programmeService.FindDegreeProgrammeFirstById(id);
            if (degreeProgramme == null)
            {
                return NotFound();
            }

            return View(degreeProgramme);
        }

        // GET: Programme/Create
        public IActionResult Create()
        {
            ViewBag.ProgrammeType = programmeTypeService.FindProgrammeTypeAll().Result;
            return View();
        }

        // POST: Programme/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Code,Title,Type")] DegreeProgramme degreeProgramme)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await programmeService.SaveDegreeProgramme(degreeProgramme);
                    var message = "Programme created successfully";
                    return RedirectToAction("Index", "Programme", new { response = message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ViewBag.Failure = "An error occurred while processing your request";
                    return View(degreeProgramme);
                }

            }
            ViewBag.Failure = "An error occurred while processing your request";
            return View(degreeProgramme);
        }

        // GET: Programme/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degreeProgramme = await programmeService.FindDegreeProgrammeById(id);
            if (degreeProgramme == null)
            {
                return NotFound();
            }
            ViewBag.ProgrammeType = programmeTypeService.FindProgrammeTypeAll().Result;
            return View(degreeProgramme);
        }

        // POST: Programme/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Code,Title,Type")] DegreeProgramme degreeProgramme)
        {
            if (id != degreeProgramme.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   programmeService.UpdateDegreeProgramme(degreeProgramme);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DegreeProgrammeExists(degreeProgramme.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var message = "Programme updated successfully";
                return RedirectToAction("Index", "Programme", new { response = message });

            }
            ViewBag.Failure = "Failed to update Programme";
            return View(degreeProgramme);
        }

        // GET: Programme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var degreeProgramme = await programmeService.FindDegreeProgrammeFirstById(id);
            if (degreeProgramme == null)
            {
                return NotFound();
            }

            return View(degreeProgramme);
        }

        // POST: Programme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var degreeProgramme = await programmeService.FindDegreeProgrammeById(id);
            programmeService.DeleteDegreeProgramme(degreeProgramme);
            return RedirectToAction(nameof(Index));
        }

        private bool DegreeProgrammeExists(int id)
        {
            return programmeService.isDegreeProgramExist(id);
        }

        public  JsonResult IsDegreeProgrammeCodeExists(string code)
        {
            List<DegreeProgramme> programmes = programmeService.FindDegreeProgrammeAll().Result.Where(a => a.Code == code).ToList();
            if (programmes.Count > 0) { return Json(false); }
            return Json(true);

        }
        public JsonResult IsDegreeProgrammeTitleExists(string title)
        {
            List<DegreeProgramme> programmes = programmeService.FindDegreeProgrammeAll().Result.Where(a => a.Title == title).ToList();
            if (programmes.Count > 0) { return Json(false); }
            return Json(true);

        }
    }
}
