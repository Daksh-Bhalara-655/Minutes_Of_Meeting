using Minutes_Of_Meeting.Controllers;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Services;

namespace Minutes_Of_Meeting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<DepartmentList>();
            builder.Services.AddScoped<MeetingTypeList>();

            builder.Services.AddScoped<Db_Connection>();
            var app = builder.Build();
          
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
