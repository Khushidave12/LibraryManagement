using LibraryManagement.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------
// 1️⃣ DATABASE (PostgreSQL for Render)
// -------------------------------------------
// Use "DefaultConnection" from appsettings.json or Render environment variables
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// -------------------------------------------
// 2️⃣ Identity (Users + Roles)
// -------------------------------------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// -------------------------------------------
// 3️⃣ JWT CONFIGURATION
// -------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

// -------------------------------------------
// 4️⃣ JSON FIX (Avoid circular loop)
// -------------------------------------------
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

// -------------------------------------------
// 5️⃣ SWAGGER WITH JWT AUTH
// -------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1"
    });

    // Enable JWT in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
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
            new string[]{ }
        }
    });
});

// -------------------------------------------
// Build App
// -------------------------------------------
var app = builder.Build();

// -------------------------------------------
// Swagger (always for Docker/Render)
// -------------------------------------------
app.UseSwagger();
app.UseSwaggerUI();

// Render does NOT use HTTPS normally
// app.UseHttpsRedirection();  <-- REMOVE for Render (optional)

// Authentication FIRST
app.UseAuthentication();
// Then Authorization
app.UseAuthorization();

// -------------------------------------------
// Redirect root → Swagger UI
// -------------------------------------------
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

// -------------------------------------------
// Map controllers
// -------------------------------------------
app.MapControllers();

app.Run();
