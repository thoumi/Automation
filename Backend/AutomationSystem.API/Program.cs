using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Serilog;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Interfaces;
using AutomationSystem.Core.Services;
using AutomationSystem.Core.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Configuration de Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/automation-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4300")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Database Configuration
var dbProvider = builder.Configuration["Database:Provider"] ?? "SqlServer";

if (dbProvider == "PostgreSQL")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
}

// Hangfire Configuration
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddHangfireServer();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSecretKeyHere_ChangeInProduction_MustBe32CharsMinimum";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "AutomationSystem";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "AutomationSystemClient";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Register Services
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<IChimeService, ChimeService>();

// HttpClient for CortexService and ChimeService
builder.Services.AddHttpClient<ICortexService, CortexService>();
builder.Services.AddHttpClient<IChimeService, ChimeService>();

// Register Jobs
builder.Services.AddScoped<RoutenverfuegbarkeitJob>();
builder.Services.AddScoped<StagingPlanJob>();
builder.Services.AddScoped<DNRUnitsJob>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

// Configuration des tâches planifiées
RecurringJob.AddOrUpdate<RoutenverfuegbarkeitJob>(
    "routenverfuegbarkeit-job",
    job => job.ExecuteAsync(),
    "25 8 * * *", // Tous les jours à 08h25
    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
);

RecurringJob.AddOrUpdate<StagingPlanJob>(
    "staging-plan-job",
    job => job.ExecuteAsync(),
    "15 9 * * *", // Tous les jours à 09h15
    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
);

RecurringJob.AddOrUpdate<DNRUnitsJob>(
    "dnr-units-job",
    job => job.ExecuteAsync(),
    "*/15 * * * *", // Toutes les 15 minutes
    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
);

app.Run();

