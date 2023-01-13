using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services
{
  public interface ICourseService
    {
        Task<CourseModule> CreateCourse(CourseModule course);
        Task<List<CourseModule>> GetAllCourses();
        Task<CourseModule> GetCourseFirstById(int? id);
        Task<CourseModule> GetCourseByCourseId(int? id);
        Task<CourseModule> UpdateCourse(CourseModule courseModule);
        Task<CourseModule> DeleteCourse(CourseModule courseModule);
        bool GetCourseIdExist(int? id);
        List<string> GetAllCourseCode();

    }

}
