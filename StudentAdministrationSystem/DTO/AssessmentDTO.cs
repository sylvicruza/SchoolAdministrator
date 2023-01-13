using StudentAdministrationSystem.Models;
using System.Collections.Generic;

namespace StudentAdministrationSystem.DTO
{
    //Holds student modules and programme assessment
    public class AssessmentDTO
    {  
        public DegreeProgramme DegreeProgramme { get; set; }
        public Student Student { get; set; }
        public int CourseCount { get; set; }
        public string Remarks { get; set; }
        public string Title { get; set; }
        public int Overall { get; set; }     
        public bool IsEnrolled { get; set; }
        public List<CourseInfo> Course { get; set; }
        public List<AssessmentInfo> Info { get; set; }
        public class AssessmentInfo
        {
            public string AssessmentName { get; set; }
            public int Marks { get; set; }
            public int Id { get; set; }

            public string CourseName { get; set; }

        }
        public class CourseInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string Type { get; set; }
            public string Remarks { get; set; }
            public int Marks { get; set; }


        }

    }
}
