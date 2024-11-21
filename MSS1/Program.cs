using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using System.Text.Json.Serialization;
using MSS1.Interfaces;
using MSS1.Repository;
using MSS1.Services;
using MSS1.Repositories;
//using AuthenticationService = MSS1.Services.AuthenticationService;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                // Enable reference handling to prevent circular reference issues
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                // Optionally, increase max depth (default is 32)
                options.JsonSerializerOptions.MaxDepth = 64;
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add DbContext with SQL Server connection string
        builder.Services.AddDbContext<ITDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("ITDBConnection"))
        );
        builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        builder.Services.AddScoped<IAuthenticServices, AuthenticService>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<ICourseService, CourseService>();

        //builder.Services.AddScoped<Microsoft.AspNetCore.Authentication.IAuthenticationService, AuthenticationService>();

        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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