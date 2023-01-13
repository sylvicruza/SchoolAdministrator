using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAdministrationSystem.Models
{
    public class DegreeProgramme
    {
       
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide Programme Code")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be six characters!")]
        [Remote("IsDegreeProgrammeCodeExists", "Programme",
             ErrorMessage = "Programme Code already exists. Please try another!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Please provide Programme Title")]
        [StringLength(50)]
        [Remote("IsDegreeProgrammeTitleExists", "Programme",
             ErrorMessage = "Programme Title already exists. Please try another!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please provide Programme Type")]
        public string Type { get; set; }

        public int? ProgrammeTypeId { get; set; }

        public virtual ProgrammeType ProgrammeType { get; set; }

        public virtual ICollection<CourseModule> CourseModules { get; set; }
    }


}

