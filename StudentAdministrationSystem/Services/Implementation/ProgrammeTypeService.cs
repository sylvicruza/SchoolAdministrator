using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services.Implementation
{
    public class ProgrammeTypeService : IProgrammeTypeService
    {
        private readonly ProgrammeTypeRepository ProgrammeTypeRepository;

        public ProgrammeTypeService(ProgrammeTypeRepository programmeTypeRepository) => ProgrammeTypeRepository = programmeTypeRepository;


        public Task<List<ProgrammeType>> FindProgrammeTypeAll() => ProgrammeTypeRepository.FindAll();

        public Task<ProgrammeType> FindProgrammeTypeById(int? id) => ProgrammeTypeRepository.FindById(id);

        public Task<ProgrammeType> FindProgrammeTypeFirstById(int? id) => ProgrammeTypeRepository.FindFirstById(id);

        public bool isProgrammeTypeExist(int? id) => ProgrammeTypeRepository.isProgrammeTypeExist(id);

        public Task<ProgrammeType> SaveProgrammeType(ProgrammeType programmeType) => ProgrammeTypeRepository.Save(programmeType);

        public void UpdateProgrammeType(ProgrammeType programmeType) => ProgrammeTypeRepository.Update(programmeType);

        public void DeleteProgrammeType(ProgrammeType programmeType) => ProgrammeTypeRepository.Delete(programmeType);
    }
}
