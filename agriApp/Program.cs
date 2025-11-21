using agriApp.Data;
using Microsoft.EntityFrameworkCore;
using agriApp.Services.Auth;

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

// ----------------------------------------
// 3️⃣ Dependency Injection (ADD HERE ❗)
// ----------------------------------------
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// ----------------------------------------
// 4️⃣ Build app
// ----------------------------------------
var app = builder.Build();

// ----------------------------------------
// 5️⃣ Swagger
// ----------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ----------------------------------------
// 6️⃣ Middleware
// ----------------------------------------
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
