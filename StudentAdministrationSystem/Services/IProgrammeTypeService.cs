using StudentAdministrationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services
{
    public interface IProgrammeTypeService
    {
        Task<List<ProgrammeType>> FindProgrammeTypeAll();

        Task<ProgrammeType> FindProgrammeTypeFirstById(int? id);

        Task<ProgrammeType> SaveProgrammeType(ProgrammeType programmeType);

        Task<ProgrammeType> FindProgrammeTypeById(int? id);

        void UpdateProgrammeType(ProgrammeType programmeType);

        void DeleteProgrammeType(ProgrammeType programmeType);

        bool isProgrammeTypeExist(int? id);
    }
}
