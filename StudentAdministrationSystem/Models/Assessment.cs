using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Models
{
    public class Assessment
    {
        public int AssessmentID { get; set; }
        public string AssessmentName { get; set; }

        [Required, Range(1, 100, ErrorMessage = "Mark must be between 1 to 100")]
        [Remote("IsAssessmentsMarkExists", "Assessments",
            ErrorMessage = "Overall mark already above 100 marks. Please entering another!")]
        public float Marks { get; set; }

        public int? CourseModuleId { get; set; }
        public virtual CourseModule Course{ get; set; }


    }
}
