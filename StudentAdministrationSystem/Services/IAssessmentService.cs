using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services
{
    public interface IAssessmentService
    {

        Task<Assessment> CreateAssessment(Assessment assessment);
        void CreateAllAssessment(List<Assessment> assessments);
        Task<List<Assessment>> GetAllAssessments();
        Task<Assessment> GetAssessmentFirstById(int? id);
        Task<Assessment> GetAssessmentByAssessmentId(int? id);
        Task<Assessment> UpdateAssessment(Assessment assessment);
        Task<Assessment> DeleteAssessment(Assessment assessment);
        bool GetAssessmentIdExist(int? id);
        List<string> GetCourseModules();

        List<CourseModule> GetAllCourses();
    }
}
