using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private CourseRepository CourseRepository;
       


        public CourseService(CourseRepository _CourseRepository)
        {
            CourseRepository = _CourseRepository;
          
         
    }


        public Task<CourseModule> CreateCourse(CourseModule course) => CourseRepository.Save(course);
       
      
        public Task<CourseModule> UpdateCourse(CourseModule courseModule)
        {
            return CourseRepository.Update(courseModule);
        }

        public Task<CourseModule> DeleteCourse(CourseModule courseModule) => CourseRepository.Delete(courseModule);


        public Task<List<CourseModule>> GetAllCourses()
        {          
            return CourseRepository.FindAll();
        }

        public Task<CourseModule> GetCourseByCourseId(int? id) => CourseRepository.FindById(id);


        public Task<CourseModule> GetCourseFirstById(int? id) => CourseRepository.FindFirstById(id);


        public bool GetCourseIdExist(int? id) => CourseRepository.FindIfExist(id);

        public List<string> GetAllCourseCode() => CourseRepository.FindByAllCourseCode();

      /*  public bool ValidateModules(int? Programme1,int? Programme2)
        {
            List<CourseModule> courses1 = CourseRepository.FindAll().Result.Where(course => course.DegreeProgrammeId == Programme1).ToList();
            List<CourseModule> courses2 = CourseRepository.FindAll().Result.Where(course => course.DegreeProgrammeId == Programme2).ToList();


        }
*/

    }
}
