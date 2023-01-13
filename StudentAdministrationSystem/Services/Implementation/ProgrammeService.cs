using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services.Implementation
{
    public class ProgrammeService : IProgrammeService
    {
        private readonly ProgrammeRepository ProgrammeRepository;

        public ProgrammeService(ProgrammeRepository programmeRepository) => ProgrammeRepository = programmeRepository;


        public Task<List<DegreeProgramme>> FindDegreeProgrammeAll() => ProgrammeRepository.FindAll();

        public Task<DegreeProgramme> FindDegreeProgrammeById(int? id) => ProgrammeRepository.FindById(id);

        public Task<DegreeProgramme> FindDegreeProgrammeFirstById(int? id) => ProgrammeRepository.FindFirstById(id);

        public bool isDegreeProgramExist(int? id) => ProgrammeRepository.isDegreeProgramExist(id);

        public Task<DegreeProgramme> SaveDegreeProgramme(DegreeProgramme degreeProgramme) => ProgrammeRepository.Save(degreeProgramme);

        public Task<DegreeProgramme> UpdateDegreeProgramme(DegreeProgramme degreeProgramme) => ProgrammeRepository.Update(degreeProgramme);

        public void DeleteDegreeProgramme(DegreeProgramme degreeProgramme) => ProgrammeRepository.Delete(degreeProgramme);
    }
}
