using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Repositories
{
    public class CourseRepository
    {
        private readonly StudentAdministrationSystemContext db;

        public CourseRepository(StudentAdministrationSystemContext db) => this.db = db;



        public async Task<List<CourseModule>> FindAll()
        {
          
            return await db.CourseModule.Include(s => s.Programme).Include(a=>a.Assessment).ToListAsync();
        }
        public async Task<CourseModule> FindFirstById(int? id) {
           
            return await SortedModule(id);

            }
        public async Task<CourseModule> Save(CourseModule courseModule)
        {
            db.Add(courseModule);
            await db.SaveChangesAsync();
            return courseModule;
        }
        public async Task<CourseModule> FindById(int? id) { 

          return  await SortedModule(id);
        }
        public async Task<CourseModule> Update(CourseModule courseModule)
        {
            db.Update(courseModule);
            await db.SaveChangesAsync();
            return courseModule;
        }
        public async Task<CourseModule> Delete(CourseModule courseModule)
        {
            db.Remove(courseModule);
            await db.SaveChangesAsync();
            return courseModule;
        }

        public  bool FindIfExist(int? id) =>  db.CourseModule.Any(m => m.ID == id);

        public List<string> FindByAllCourseCode() => db.CourseModule.Select(m => m.CourseCode).ToList();

        private Task<CourseModule> SortedModule(int? id)
        {
            var assessments = db.Assessment.ToList().Where(a => a.CourseModuleId == id);
            CourseModule courseModule = db.CourseModule.Include(s => s.Programme).FirstOrDefault(m => m.ID == id);
            courseModule.Assessment = assessments.ToList();
            return Task.FromResult(courseModule);
        }

       
        

    }
}
