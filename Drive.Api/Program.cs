using Drive.Business.Services;
using Drive.Business.UseCases;
using Drive.Domain.Interfaces;
using Drive.Infrastructure.Data;
using Drive.Infrastructure.Repositories;
using Drive.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register EF Core DbContext
builder.Services.AddDbContext<DriveDbContext>(options => options.UseInMemoryDatabase("DriveDb"));

// Register application services
builder.Services.AddScoped<IFileRepository, DriveFileRepository>();
builder.Services.AddSingleton<IFileStorage>(_ => new LocalFileStorage());

// Register use case handlers
builder.Services.AddScoped<UploadFileHandler>();
builder.Services.AddScoped<GetFileHandler>();
builder.Services.AddScoped<DeleteFileHandler>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
