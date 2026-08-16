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
        builder.Services.AddInfrastructure(builder.Configuration);

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

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                "Bearer",
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Digite: Bearer {seu token JWT}"
                });

            options.AddSecurityRequirement(
                new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
                });
        });

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

        // Authentication
        app.UseAuthentication();

        // Authorization
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}