using Microsoft.AspNetCore.Mvc;

namespace StudentAdministrationSystem.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        
        public int DegreeProgrammeId { get; set; }

      /*  [Remote("IsModuleAlreadyExists", "StudentCourses", AdditionalFields = "StudentId",
          ErrorMessage = "Module already assigned for Student. Please try another!")]*/
        public int CourseModuleId { get; set; }

        public int? AssessmentId { get; set; }   

        public float Mark { get; set; }

        public string Remarks { get; set; }

        public virtual Student Student { get; set; }
        public virtual DegreeProgramme DegreeProgramme { get; set; }
        public virtual CourseModule CourseModule { get; set; }
        public virtual Assessment Assessment { get; set; }

    }
}
