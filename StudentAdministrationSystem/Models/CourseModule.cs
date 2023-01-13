using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Models
{
    public class CourseModule
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "please provide Course Title")]
        [Remote("IsCourseTitleExists", "CourseModules",
            ErrorMessage = "Course Title already exists. Please try another!")]
        public string CourseTitle { get; set; }

        [Required(ErrorMessage = "Please provide Course Code")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Code must five characters long!")]
        [Remote("IsCourseCodeExists", "CourseModules",
            ErrorMessage = "Course Code already exists. Please try another!")]
        public string CourseCode { get; set; }

        public string CourseType { get; set; }

        [Required(ErrorMessage = "Please Select Programme")]
        public int DegreeProgrammeId { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Mark must be between 1 to 100")]
        public int Marks { get; set; }

        public virtual DegreeProgramme Programme { get; set; }

        public virtual ICollection<Assessment> Assessment { get; set; }
    }
}
