using Microsoft.EntityFrameworkCore;
using UsersTasksAPI.Models;


var builder = WebApplication.CreateBuilder(args);

// EF Core will use SQLite and connection string from appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Build App - creates WebApp object that we will run
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger for API Documentation
    app.MapOpenApi();
}

// Force Https
app.UseHttpsRedirection();

// Map controller routes so the API knows how to handle requests
app.MapControllers();

app.Run();

