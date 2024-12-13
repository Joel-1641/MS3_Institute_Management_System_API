using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSS1.Database;
using MSS1.Interfaces;
using MSS1.Repository;
using MSS1.Repositories;
using MSS1.Services;
using System.Text;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Ensure the configuration includes appsettings.json
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();


        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Enable reference handling to prevent circular reference issues
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // Add SwaggerGen
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Read JWT settings from appsettings.json
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = jwtSettings["Key"];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException("JWT Key is not configured in appsettings.json");
        }
        var keyBytes = Encoding.ASCII.GetBytes(key);

        // Register the JWT key as a singleton so it can be injected into services
        builder.Services.AddSingleton(key); // Register the JWT key string

        // Add JWT Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)  // Use the key bytes here
                };
            });

        // Register DbContext with SQL Server connection string
        builder.Services.AddDbContext<ITDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("ITDBConnection"))
        );

        // Register Scoped Services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IStudentRepository, StudentRepository>();
        builder.Services.AddScoped<IStudentService, StudentService>();
        builder.Services.AddScoped<IContactUsService, ContactUsService>();
        builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
        builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
        builder.Services.AddScoped<ILecturerService, LecturerService>();
        builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
        builder.Services.AddScoped<IStudentCourseService, StudentCourseService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IEmailService, EmailService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Enable authentication middleware
        app.UseAuthentication();

        // Enable authorization middleware
        app.UseAuthorization();

        // Configure CORS
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });

        // Map controllers
        app.MapControllers();

        app.Run();
    }
}
