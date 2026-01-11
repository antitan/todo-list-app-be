using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDoList.EntityFramework;

namespace ToDoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
             
            builder.Services.AddControllers();
             
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
             
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactDev", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173", "https://localhost:7164")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));


            var app = builder.Build();

           

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();      // wwwroot 

            app.UseCors("ReactDev");

            app.MapControllers();

            // Fallback React: toutes les routes non-API -> index.html
            app.MapFallbackToFile("index.html"); 

            app.Run();

           
        }
    }
}
