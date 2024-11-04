using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace StudentAdministrationSystem.DTO
{
    public enum Grade
    {
        [Description("UNDEFINED")] UNDEFINED,
        [Description("PASS")] PASS,
        [Description("PASS COMPENSATION")] PASS_COMPENSATION,
        [Description("FAIL")] FAIL,
        [Description("DISTINCTION")] DISTINCTION
    }

    public static class GradeExtensions
    {
        // Retrieves the description attribute of the Grade enum
        public static string GetDescription(this Grade grade)
        {
            var memberInfo = grade.GetType().GetMember(grade.ToString()).FirstOrDefault();
            var descriptionAttribute = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? grade.ToString();
        }
    }

    // Manages grading operations for students based on scores
    public class GradeStudent
    {
        private readonly StudentAdministrationSystemContext _context;

        public GradeStudent(StudentAdministrationSystemContext context)
        {
            _context = context;
        }

        // Determines grade for module score
        public string CheckModuleGrade(int score)
        {
            if (score >= 50) return Grade.PASS.GetDescription();
            if (score >= 45) return Grade.PASS_COMPENSATION.GetDescription();
            return Grade.FAIL.GetDescription();
        }

        // Determines grade for program score
        public string CheckProgrammeGrade(int score)
        {
            if (score >= 70) return Grade.DISTINCTION.GetDescription();
            return score >= 50 ? Grade.PASS.GetDescription() : Grade.FAIL.GetDescription();
        }

        // Checks if a student's mark is undefined for any course
        public string CalculateUndefinedPassMark(int studentId)
        {
            var studentCourses = StudentCoursesByStudentId(studentId);
            return studentCourses.Any(sc => sc.Mark == 0) ? Grade.UNDEFINED.GetDescription() : string.Empty;
        }

        // Calculates the final result of a student based on their courses and program requirements
        public AssessmentDTO FinalResult(int studentId)
        {
            var studentCourses = StudentCoursesByStudentId(studentId).ToList();
            var result = new AssessmentDTO
            {
                Student = studentCourses.FirstOrDefault()?.Student,
                DegreeProgramme = studentCourses.FirstOrDefault()?.DegreeProgramme
            };

            var courses = studentCourses
                .GroupBy(sc => sc.CourseModuleId)
                .Select(g => new AssessmentDTO.CourseInfo
                {
                    Id = g.Key,
                    Name = g.First().CourseModule.CourseTitle,
                    Code = g.First().CourseModule.CourseCode,
                    Marks = (int)g.Sum(sc => sc.Mark),
                    Remarks = CheckModuleGrade((int)g.Sum(sc => sc.Mark))
                })
                .ToList();

            result.Course = courses;
            result.CourseCount = courses.Count();
            result.IsEnrolled = EnrollmentStatus(result.DegreeProgramme.ProgrammeType.NumberOfCourseModule, result.CourseCount);
            result.Overall = courses.Any() ? courses.Sum(c => c.Marks) / courses.Count : 0;
            result.Remarks = CalculateUndefinedPassMark(result.Student.Id) == Grade.UNDEFINED.GetDescription()
                ? Grade.UNDEFINED.GetDescription()
                : CheckProgrammeGrade(result.Overall);

            return result;
        }

        // Retrieves all students' assessments
        public List<AssessmentDTO> GetAllStudentResults()
        {
            var allStudentCourses = StudentCourses()
                .GroupBy(sc => sc.StudentId)
                .Select(g => g.First().StudentId)
                .ToList();

            return allStudentCourses.Select(FinalResult).ToList();
        }

        // Checks if the student's enrollment meets the program's module requirement
        private bool EnrollmentStatus(int expectedModules, int enrolledModules) => expectedModules == enrolledModules;

        // Retrieves courses of a specific student by student ID
        private IEnumerable<Enrollment> StudentCoursesByStudentId(int studentId) =>
            StudentCourses().Where(sc => sc.StudentId == studentId);

        // Retrieves all enrollments with related entities
        private IQueryable<Enrollment> StudentCourses() =>
            _context.Enrollment
                .Include(e => e.CourseModule)
                .Include(e => e.DegreeProgramme.ProgrammeType)
                .Include(e => e.Student)
                .Include(e => e.Assessment);
    }
}
