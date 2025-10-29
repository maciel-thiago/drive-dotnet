using System.Text;
using Drive.Business.Auth;
using Drive.Business.Services;
using Drive.Business.UseCases;
using Drive.Domain.Auth;
using Drive.Domain.Interfaces;
using Drive.Infrastructure.Auth;
using Drive.Infrastructure.Data;
using Drive.Infrastructure.Repositories;
using Drive.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register EF Core DbContext
builder.Services.AddDbContext<DriveDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register application services
builder.Services.AddScoped<IFileRepository, DriveFileRepository>();
builder.Services.AddSingleton<IFileStorage>(_ => new LocalFileStorage());

// Register use case handlers
builder.Services.AddScoped<UploadFileHandler>();
builder.Services.AddScoped<GetFileHandler>();
builder.Services.AddScoped<DeleteFileHandler>();
builder.Services.AddScoped<RestoreFileHandler>();
builder.Services.AddScoped<ListDeletedFilesHandler>();
builder.Services.AddScoped<GetFilesByUserHandler>();
builder.Services.AddScoped<LoginHandler>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["SigningKey"];
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
