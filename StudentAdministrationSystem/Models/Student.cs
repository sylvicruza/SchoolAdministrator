using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Remote("IsStudentEmailExists", "Student", ErrorMessage = "Email already exists! Please try another.")]
        public string Email { get; set; }
        public string ContactNo { get; set; }

        public string Address { get; set; }

        //Academic year
        public string Cohort { get; set; }

        public int DegreeProgrammeId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public virtual DegreeProgramme DegreeProgramme { get; set; }
        public virtual ICollection<CourseModule> Courses { get; set; }
    }
}
