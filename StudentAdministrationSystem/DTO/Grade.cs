using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using static StudentAdministrationSystem.DTO.AssessmentDTO;

namespace StudentAdministrationSystem.DTO
{
    public enum Grade
    {
        //Defines the expected grades
        [Description("UNDEFINED")] UNDEFINED,
        [Description("PASS")] PASS,
        [Description("PASSCOMPENSATION")] PASS_COMPENSATION,
        [Description("FAIL")] FAIL,
        [Description("DISTINCTION")] DISTINCTION

    }

    public static class GradeStatus
    {
        //Get the description of the grades
        public static string GetDescription(this Grade GetString)
        {
            Type genericEnumType = GetString.GetType();
            MemberInfo[] member = genericEnumType.GetMember(GetString.ToString());
            if ((member.Length <= 0)) return GetString.ToString();
            object[] custom = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return custom.Any() ? ((DescriptionAttribute)custom.ElementAt(0)).Description : GetString.ToString();
        }
    }
    //Handles Grade of Student based on the student marks
    public class GradeStudent
    {
        private readonly StudentAdministrationSystemContext _context;

        public GradeStudent(StudentAdministrationSystemContext context)
        {
            _context = context;
        }


        //Grade check for modules score
        public string CheckModuleGrade(int score)
        {
            if (score >= 50)
            {
                return Grade.PASS.GetDescription();
            }
            else if (score < 50 && score >= 45)
            {
                return Grade.PASS_COMPENSATION.GetDescription();
            }
            else
            {
                return Grade.FAIL.GetDescription();
            }

        }
        //Grade check for programme score
        public string CheckProgrammeGrade(int score)
        {
            if (score >= 50 && score < 70)
            {
                return Grade.PASS.GetDescription();
            }
            else if (score >= 70)
            {
                return Grade.DISTINCTION.GetDescription();
            }
            else
            {
                return Grade.FAIL.GetDescription();
            }

        }
        //Grade check for undefined value if student score is not yet assigned
        public string CalculateUndefinedPassMark(int studentId)
        {
            var studentCourses = StudentCoursesByStudentId(studentId);
            if (studentCourses != null)
            {
                foreach (var student in studentCourses)
                {

                    if (student.Mark == 0)
                    {
                        return Grade.UNDEFINED.GetDescription();
                    }
                }
            }
            return "";
        }
        //Get the scores of student modules and overall programme using studentId
        public AssessmentDTO FinalResult(int studentId)
        {
            AssessmentDTO result = new AssessmentDTO();

            var Result = StudentCoursesByStudentId(studentId);
            result.Student = Result.First().Student;
            result.DegreeProgramme = Result.First().DegreeProgramme;
            List<CourseInfo> courses = new List<CourseInfo>();

            var courseIds = Result.Select(s => s.CourseModuleId).Distinct().ToList();

            foreach (var cid in courseIds)
            {
                var courseLst = Result
                .Where(m => m.CourseModuleId == cid);
                if (courseLst.Count() > 0)
                {
                    CourseInfo course = new CourseInfo
                    {

                        Code = courseLst.First().CourseModule.CourseCode,
                        Marks = (int)courseLst.Sum(i => i.Mark),
                        Name = courseLst.First().CourseModule.CourseTitle,
                        Id = cid
                    };
                  //  course.Remarks = CalculateUndefinedPassMark(result.Student.Id) == Grade.UNDEFINED.GetDescription() ? CalculateUndefinedPassMark(result.Student.Id) : CheckModuleGrade(course.Marks);
                    course.Remarks = CheckModuleGrade(course.Marks);
                    courses.Add(course);

                }
            }
            result.Course = courses;
            result.CourseCount = courses.Count();
            result.IsEnrolled = EnrollmentStatus(result.Student.DegreeProgramme.ProgrammeType.NumberOfCourseModule,result.CourseCount);
            result.Overall = ((int)courses.Sum(i => i.Marks)) / courses.Count;
            result.Remarks = CalculateUndefinedPassMark(result.Student.Id) == Grade.UNDEFINED.GetDescription() ? CalculateUndefinedPassMark(result.Student.Id) : CheckProgrammeGrade(result.Overall);
            return result;
        }
        //Get the scores of all student modules and overall programme
        public List<AssessmentDTO> AllStudent()
        {
            List<AssessmentDTO> result = new List<AssessmentDTO>();
            var AllstudentCourses = StudentCourses().GroupBy(x => x.StudentId).Select(x => x.First()).ToList();
            foreach(var studentCourse in AllstudentCourses)
            {
                AssessmentDTO report = FinalResult(studentCourse.StudentId);
                result.Add(report);
            }
            return result;
        }
        //Check if student is enrolled number of expected modules per number of modules enrolled
        private bool EnrollmentStatus(int expectedNumberOfModules,int numberOfModulesEnrolled)
        {
            return expectedNumberOfModules == numberOfModulesEnrolled;         
        }


        public IEnumerable<Enrollment> StudentCoursesByStudentId(int studentId)
        {
          return StudentCourses().Where(a => a.StudentId == studentId);
        }

        public IEnumerable<Enrollment> StudentCourses()
        {
            return _context.Enrollment
                .Include(s => s.CourseModule)
                .Include(s => s.DegreeProgramme.ProgrammeType)
                .Include(s => s.Student)
                .Include(s => s.Assessment);
            
        }
    }
}
