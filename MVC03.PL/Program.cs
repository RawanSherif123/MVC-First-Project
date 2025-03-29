using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVC03.BLL;
using MVC03.BLL.Interfaces;
using MVC03.BLL.Repositories;
using MVC03.DAL.Data.Contexts;
using MVC03.DAL.Models;
using MVC03.PL.Mapping;
using MVC03.PL.Services;

namespace MVC03.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency Injection for DepartmentRepository 
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allow Dependency Injection for EmployeeRepository 

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection for CompanyDbContext


            //Life Time
            //builder.Services.AddScoped();
            //builder.Services.AddTransient();
            //builder.Services.AddSingleton();

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                             .AddEntityFrameworkStores<CompanyDbContext>()
                             .AddDefaultTokenProviders();



            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
                
            });

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddScoped<ISingletonService, SingletonService>();
            builder.Services.AddScoped<ITransentService, TransentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
