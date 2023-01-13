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
    public class ProgrammeTypeController : Controller
    {
        private readonly IProgrammeTypeService programmeTypeService;

        public ProgrammeTypeController(IProgrammeTypeService ProgrammeTypeService)
        {
            programmeTypeService = ProgrammeTypeService;
        }

        // GET: ProgrammeType
        public async Task<IActionResult> Index(string response)
        {
            ViewBag.Message = response;
            return View(await programmeTypeService.FindProgrammeTypeAll());
        }

        // GET: ProgrammeType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programmeType = await programmeTypeService.FindProgrammeTypeFirstById(id);
            if (programmeType == null)
            {
                return NotFound();
            }

            return View(programmeType);
        }

        // GET: ProgrammeType/Create
        public IActionResult Create()
        {
         
            return View();
        }

        // POST: ProgrammeType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NumberOfCourseModule")] ProgrammeType programmeType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await programmeTypeService.SaveProgrammeType(programmeType);
                    var message = "ProgrammeType created successfully";
                    return RedirectToAction("Index", "ProgrammeType", new { response = message });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ViewBag.Failure = "An error occurred while processing your request";
                    return View(programmeType);
                }
            }
            ViewBag.Failure = "An error occurred while processing your request";
            return View(programmeType);
        }

        // GET: ProgrammeType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programmeType = await programmeTypeService.FindProgrammeTypeById(id);
            if (programmeType == null)
            {
                return NotFound();
            }
            return View(programmeType);
        }

        // POST: ProgrammeType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfCourseModule")] ProgrammeType programmeType)
        {
            if (id != programmeType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    programmeTypeService.UpdateProgrammeType(programmeType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgrammeTypeExists(programmeType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var message = "ProgrammeType updated successfully";
                return RedirectToAction("Index", "ProgrammeType", new { response = message });
            }
            ViewBag.Failure = "Failed to update ProgrammeType";
            return View(programmeType);
        }

        // GET: ProgrammeType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programmeType = await programmeTypeService.FindProgrammeTypeFirstById(id);
            if (programmeType == null)
            {
                return NotFound();
            }

            return View(programmeType);
        }

        // POST: ProgrammeType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var programmeType = await programmeTypeService.FindProgrammeTypeById(id);
            programmeTypeService.DeleteProgrammeType(programmeType);
            return RedirectToAction(nameof(Index));
        }

        private bool ProgrammeTypeExists(int id)
        {
            return programmeTypeService.isProgrammeTypeExist(id);
        }
    }
}
