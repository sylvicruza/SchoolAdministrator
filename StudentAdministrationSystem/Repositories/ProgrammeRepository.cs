using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Repositories
{
    public class ProgrammeRepository
    {
        private readonly StudentAdministrationSystemContext db;

        public ProgrammeRepository(StudentAdministrationSystemContext context) => db = context;

        public async Task<List<DegreeProgramme>> FindAll()
        {
           return await db.DegreeProgramme.ToListAsync();
        }
        public async Task<DegreeProgramme> FindFirstById(int? id) => await db.DegreeProgramme.Include(p=>p.ProgrammeType).Include(a=>a.CourseModules).FirstOrDefaultAsync(m => m.ID == id);

        public async Task<DegreeProgramme> Save(DegreeProgramme degreeProgramme)
        {
            //Code,Title,Type
            var degreeObj = new DegreeProgramme();
            degreeObj.Code = degreeProgramme.Code;
            degreeObj.Title = degreeProgramme.Title;
            degreeObj.Type = degreeProgramme.Type;
            degreeObj.ProgrammeTypeId = Int32.Parse(degreeProgramme.Type);

            db.Add(degreeObj);
            await db.SaveChangesAsync();
            return degreeProgramme;
        }
        public async Task<DegreeProgramme> FindById(int? id) => await db.DegreeProgramme.FindAsync(id);

        public async Task<DegreeProgramme> Update(DegreeProgramme degreeProgramme)
        {
            //Code,Title,Type
            var degreeObj = degreeProgramme;
            degreeObj.ProgrammeTypeId = Int32.Parse(degreeProgramme.Type);
            db.Update(degreeObj);
            await db.SaveChangesAsync();
            return degreeProgramme;
        }
        public async void Delete(DegreeProgramme degreeProgramme)
        {
            db.DegreeProgramme.Remove(degreeProgramme);
            await db.SaveChangesAsync();
        }
        public bool isDegreeProgramExist(int? id)=>db.DegreeProgramme.Any(e => e.ID == id);
    }
}
