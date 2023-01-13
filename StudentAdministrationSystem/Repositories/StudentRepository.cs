using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAdministrationSystem.Repositories
{
    public class StudentRepository
    {
        private readonly StudentAdministrationSystemContext db;

        public StudentRepository(StudentAdministrationSystemContext db)
        {
            this.db = db;
        }


        public async Task<List<Student>> FindAll()
        {
            var studentAdministrationSystemContext = db.Student.Include(s => s.DegreeProgramme);
            return await studentAdministrationSystemContext.ToListAsync();
        }

        public IEnumerable<SelectListItem> FindSelectedList()
        {
            return new SelectList(db.DegreeProgramme, "ID", "Title");

        }

        public IEnumerable<SelectListItem> FindStudentSelectedList(Student student)
        {
            return new SelectList(db.DegreeProgramme, "ID", "Title", student.DegreeProgrammeId);
        }

        public async Task<Student> Save(Student student) {
            db.Add(student);
            await db.SaveChangesAsync();
            return student;
        }
        public async Task<Student> Update(Student student)
        {
            db.Update(student);
            await db.SaveChangesAsync();
            return student;
        }

        public Task<Student> FindFirstById(int? id)
        {
            return db.Student
                .Include(s => s.DegreeProgramme.CourseModules)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Student> FindById(int? id) { 
       return await db.Student.FindAsync(id);
    }

        public async void Delete(Student student)
        {

            db.Student.Remove(student);
            await db.SaveChangesAsync();
        }

        public bool isStudentExist(int? id)
        {
          return  db.Student.Any(e => e.Id == id);
        }


      

    }

   
}
