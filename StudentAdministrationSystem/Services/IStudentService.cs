using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAdministrationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Services
{
    public interface IStudentService
    {
        Task<List<Student>> FindAllStudents();
        IEnumerable<SelectListItem> FindSelectedList();
        IEnumerable<SelectListItem> FindStudentSelectedList(Student student);
        void SaveStudent(Student student);
        Task<Student> UpdateStudent(Student student);
        Task<Student> FindStudentFirstById(int? id);
        Task<Student> FindStudentById(int? id);
        void DeleteStudent(Student student);

        bool isStudentExist(int? id);
    }
}
