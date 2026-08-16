using Gardula.Api.Exceptions;
using Gardula.Application;
using Gardula.Infrastructure;
using Gardula.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gardula.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // Problem Details
        builder.Services.AddProblemDetails();

        // Global Exception Handler
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        // Application
        builder.Services.AddApplication();

        // Infrastructure
        builder.Services.AddInfrastructure();

        // Database
        builder.Services.AddDbContext<GardulaDbContext>(options =>
        {
            var connectionString = builder.Configuration
                .GetConnectionString("GardulaDatabase");

            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));
        });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Global Exception Handler
        app.UseExceptionHandler();

        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}