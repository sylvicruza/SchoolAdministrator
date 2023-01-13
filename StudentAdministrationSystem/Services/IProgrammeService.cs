using StudentAdministrationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services
{
    public interface IProgrammeService
    {
        Task<List<DegreeProgramme>> FindDegreeProgrammeAll();

        Task<DegreeProgramme> FindDegreeProgrammeFirstById(int? id);

        Task<DegreeProgramme> SaveDegreeProgramme(DegreeProgramme degreeProgramme);

        Task<DegreeProgramme> FindDegreeProgrammeById(int? id);

        Task<DegreeProgramme> UpdateDegreeProgramme(DegreeProgramme degreeProgramme);

        void DeleteDegreeProgramme(DegreeProgramme degreeProgramme);

        bool isDegreeProgramExist(int? id);
    }
}
