using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAdministrationSystem.Models;
using StudentAdministrationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services.Implementation
{
    public class StudentService : IStudentService
       
    {
        private readonly StudentRepository studentRepository;

        public StudentService(StudentRepository StudentRepository) => this.studentRepository = StudentRepository;


        public void DeleteStudent(Student student) => studentRepository.Delete(student);

        public Task<List<Student>> FindAllStudents() => studentRepository.FindAll();

        public IEnumerable<SelectListItem> FindSelectedList() => studentRepository.FindSelectedList();

        public Task<Student> FindStudentById(int? id) => studentRepository.FindById(id);

        public Task<Student> FindStudentFirstById(int? id) => studentRepository.FindFirstById(id);

        public IEnumerable<SelectListItem> FindStudentSelectedList(Student student) => studentRepository.FindStudentSelectedList(student);

        public bool isStudentExist(int? id) => studentRepository.isStudentExist(id);

        public void SaveStudent(Student student) {
            var count = studentRepository.FindAll().Result.Count;
            var Today = DateTime.Now;
            student.EnrollmentDate = Today;
            student.Cohort = Today.Year.ToString();

            if (count == 0)
            {
                student.StudentNumber = Today.Year.ToString() + 1.ToString("D6");

            }
            else
            {
                count++;
                student.StudentNumber = Today.Year.ToString() + count.ToString("D6");

            }
            studentRepository.Save(student); 
        }

        public Task<Student> UpdateStudent(Student student) => studentRepository.Update(student);
    }
}
