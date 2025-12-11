using BackendService.DAL.Interfaces;
using BackendService.DAL.Logics;
using BackendService.DAL.Models;
using BackendService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BackendService.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            // Add services to the container.\
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(
                option =>
                {
                    option.UseNpgsql(config.GetConnectionString(nameof(ApplicationDbContext)));
                });

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();

            builder.Services.AddScoped<IPostLogic, PostLogic>();
            builder.Services.AddScoped<ITagLogic, TagLogic>();

            builder.Services.AddProblemDetails();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
