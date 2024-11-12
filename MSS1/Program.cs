using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MSS1.Database;
using System.Text.Json;
using System.Text.Json.Serialization;

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
