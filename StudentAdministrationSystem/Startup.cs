using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAdministrationSystem.Data;
using StudentAdministrationSystem.Services;
using StudentAdministrationSystem.Services.Implementation;
using StudentAdministrationSystem.Controllers;
using StudentAdministrationSystem.Repositories;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace StudentAdministrationSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();

            //Dependency Injection for ProgrammeType.
            services.AddScoped<StudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();

            //Dependency Injection for ProgrammeType.
            services.AddScoped<ProgrammeTypeRepository, ProgrammeTypeRepository>();
            services.AddScoped<IProgrammeTypeService, ProgrammeTypeService>();


            //Dependency Injection for Programme.
            services.AddScoped<ProgrammeRepository, ProgrammeRepository>();
            services.AddScoped<IProgrammeService, ProgrammeService>();

            //Dependency Injection for Student.
            services.AddScoped<StudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();

            //Dependency Injection for Course.
            services.AddScoped<CourseRepository,CourseRepository>();
            services.AddScoped<ICourseService,CourseService>();

            //Dependency Injection for Assessment.
            services.AddScoped<AssessmentRepository, AssessmentRepository>();
            services.AddScoped<IAssessmentService, AssessmentService>();

            services.AddDbContext<StudentAdministrationSystemContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("StudentAdministrationSystemContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
         

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
