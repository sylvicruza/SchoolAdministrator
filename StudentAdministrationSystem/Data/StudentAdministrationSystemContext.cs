using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Models;

namespace StudentAdministrationSystem.Data
{
    public class StudentAdministrationSystemContext : DbContext
    {
        public StudentAdministrationSystemContext (DbContextOptions<StudentAdministrationSystemContext> options)
            : base(options)
        {
        }

        public DbSet<StudentAdministrationSystem.Models.Student> Student { get; set; }

        public DbSet<StudentAdministrationSystem.Models.CourseModule> CourseModule { get; set; }

        public DbSet<StudentAdministrationSystem.Models.Assessment> Assessment { get; set; }

        public DbSet<StudentAdministrationSystem.Models.DegreeProgramme> DegreeProgramme { get; set; }

        public DbSet<StudentAdministrationSystem.Models.ProgrammeType> ProgrammeType { get; set; }

        public DbSet<StudentAdministrationSystem.Models.Enrollment> Enrollment { get; set; }
    }
}
