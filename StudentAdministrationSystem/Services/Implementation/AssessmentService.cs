using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services.Implementation
{
    public class AssessmentService : IAssessmentService
    {
        private AssessmentRepository AssessmentRepository;
        private ICourseService CourseService;
        private StudentAdministrationSystemContext db;


        public AssessmentService(AssessmentRepository _AssessmentRepository, ICourseService _CourseService, StudentAdministrationSystemContext context)
        {
            AssessmentRepository = _AssessmentRepository;
            CourseService = _CourseService;
            db = context;
        }

        public void CreateAllAssessment(List<Assessment> assessments) => AssessmentRepository.SaveAll(assessments);

        public Task<Assessment> CreateAssessment(Assessment assessment)
        {

            return AssessmentRepository.Save(assessment);
        }

        public Task<Assessment> DeleteAssessment(Assessment assessment) => AssessmentRepository.Delete(assessment);
        public Task<List<Assessment>> GetAllAssessments(){
           
            return AssessmentRepository.FindAll();
            }
     




        public Task<Assessment> GetAssessmentByAssessmentId(int? id) => AssessmentRepository.FindById(id);

        public Task<Assessment> GetAssessmentFirstById(int? id) => AssessmentRepository.FindById(id);

        public bool GetAssessmentIdExist(int? id) => AssessmentRepository.FindIfExist(id);

        public Task<Assessment> UpdateAssessment(Assessment assessment)
        {
            return AssessmentRepository.Update(assessment);
        }
        public  List<string> GetCourseModules() =>  CourseService.GetAllCourseCode();

        public List<CourseModule> GetAllCourses()
        {
            return CourseService.GetAllCourses().Result;
        }

        
    }
}
