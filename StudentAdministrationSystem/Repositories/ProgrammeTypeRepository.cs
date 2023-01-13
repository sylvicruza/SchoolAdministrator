using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Repositories
{
    public class ProgrammeTypeRepository
    {
        private readonly StudentAdministrationSystemContext db;

        public ProgrammeTypeRepository(StudentAdministrationSystemContext context) => db = context;

        public async Task<List<ProgrammeType>> FindAll() => await db.ProgrammeType.ToListAsync();
        public async Task<ProgrammeType> FindFirstById(int? id) => await db.ProgrammeType.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<ProgrammeType> Save(ProgrammeType programmeType)
        {
            db.Add(programmeType);
            await db.SaveChangesAsync();
            return programmeType;
        }
        public async Task<ProgrammeType> FindById(int? id) => await db.ProgrammeType.FindAsync(id);

        public async void Update(ProgrammeType programmeType)
        {
            db.Update(programmeType);
            await db.SaveChangesAsync();
        }
        public async void Delete(ProgrammeType programmeType)
        {
            db.ProgrammeType.Remove(programmeType);
            await db.SaveChangesAsync();
        }
        public bool isProgrammeTypeExist(int? id) => db.ProgrammeType.Any(e => e.Id == id);
    }
}
