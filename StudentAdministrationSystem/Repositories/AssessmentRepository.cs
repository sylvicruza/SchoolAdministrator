using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Repositories
{
    public class AssessmentRepository
    {
        private readonly StudentAdministrationSystemContext db;

        public AssessmentRepository(StudentAdministrationSystemContext db) => this.db = db;




        public async Task<List<Assessment>> FindAll()
        {
          
            
         return await db.Assessment.Include(a =>a.Course.Programme).ToListAsync();
   
        }
        public async Task<Assessment> FindFirstById(int? id) => await db.Assessment.FirstOrDefaultAsync(m => m.AssessmentID == id);
        public async Task<Assessment> Save(Assessment assessment)
        {
            db.Add(assessment);
            await db.SaveChangesAsync();
            return assessment;
        }

        public async void SaveAll(List<Assessment> assessment)
        {
            db.Assessment.AddRange(assessment);
            await db.SaveChangesAsync();
           
        }

        public async Task<Assessment> FindById(int? id) => await db.Assessment.FindAsync(id);
        public async Task<Assessment> Update(Assessment assessment)
        {
            db.Update(assessment);
            await db.SaveChangesAsync();
            return assessment;
        }
        public async Task<Assessment> Delete(Assessment assessment)
        {
            db.Remove(assessment);
            await db.SaveChangesAsync();
            return assessment;
        }

        public bool FindIfExist(int? id) => db.Assessment.Any(m => m.AssessmentID == id);

    }


}
