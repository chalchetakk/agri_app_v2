using agriApp.Data;
using Microsoft.EntityFrameworkCore;
using agriApp.Services.Auth;
using agriApp.Services.Roles;
using agriApp.Services.Farmers;
using agriApp.Services.Crops;
using agriApp.Services.Buyers;
using agriApp.Services.Sellers;
using agriApp.Services.MandiOfficials;
using agriApp.Services.Mandis;
using agriApp.Services.Lots;
using agriApp.Services.Files;
using agriApp.Services.Auctions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;  // ⭐ Add this
using System.Security.Claims; // ⭐ Add this namespace
using System.Text;

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
// 2️⃣ JWT AUTHENTICATION
// ----------------------------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // you can set true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,   // no delay for token expiration
        ValidateIssuerSigningKey = true,

        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        // NameClaimType = JwtRegisteredClaimNames.Sub,   // ⭐ map "sub"
    // RoleClaimType = "role"
    
    // NameClaimType = ClaimTypes.NameIdentifier,   // ⭐ map Name
//  NameClaimType = ClaimTypes.NameIdentifier // ⭐ Map JWT "sub" to User.Identity.Name
// NameClaimType = JwtRegisteredClaimNames.Sub   // map "sub" to User.Identity.Name
NameClaimType = JwtRegisteredClaimNames.Sub, // ⭐ ensures Name = "sub"
          RoleClaimType = ClaimTypes.Role // IMPORTANT
    };
});

// ----------------------------------------
// 3️⃣ Add Controllers + Swagger
// ----------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "agriApp API", Version = "v1" });

    // JWT Authorization in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ----------------------------------------
// 4️⃣ Dependency Injection
// ----------------------------------------
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IFarmerService, FarmerService>();
builder.Services.AddScoped<ICropService, CropService>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IMandiOfficialService, MandiOfficialService>();
builder.Services.AddScoped<IMandiService, MandiService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<ILotService, LotService>();
// LOT SERVICES
builder.Services.AddScoped<IArrivedLotService, ArrivedLotService>();
builder.Services.AddScoped<ILiveAuctionLotService, LiveAuctionLotService>();

builder.Services.AddSingleton<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<IBuyerInterestLotService, BuyerInterestLotService>();
builder.Services.AddScoped<IBuyerLotRecommendationService, BuyerLotRecommendationService>();

builder.Services.AddScoped<IAuctionService, AuctionService>();

// ----------------------------------------
// 5️⃣ Build app
// ----------------------------------------
var app = builder.Build();

// ----------------------------------------
// 6️⃣ Swagger
// ----------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ----------------------------------------
// 7️⃣ Middleware pipeline
// ----------------------------------------
app.UseHttpsRedirection();

app.UseAuthentication();  // ⭐ REQUIRED BEFORE UseAuthorization()
app.UseAuthorization();


// Later in the app setup
app.UseCors("AllowAll");
app.MapControllers();


app.Run();
