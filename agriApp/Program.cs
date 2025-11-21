using agriApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------
// 1️⃣ Add DbContext (PostgreSQL)
// ----------------------------------------

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AgriDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// ----------------------------------------
// 2️⃣ Add services (controllers, etc.)
// ----------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ----------------------------------------
// 3️⃣ Swagger for API testing
// ----------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ----------------------------------------
// 4️⃣ Middleware pipeline
// ----------------------------------------

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
