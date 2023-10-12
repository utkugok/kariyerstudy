using FluentValidation;
using Microsoft.AspNetCore.OutputCaching;
using study.DTOs;
using study.Extensions;
using study.Models;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services;
using study.Services.Interfaces;
using study.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElastic(builder.Configuration);

builder.Services.AddScoped<IValidator<CompanyCreateDto>, CompanyValidator>();
builder.Services.AddScoped<IValidator<JobCreateDto>, JobValidator>();

builder.Services.AddOutputCache();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IProhibitedWordsService, ProhibitedWordsService>();
builder.Services.AddScoped<IProhibitedWordsRepository, ProhibitedWordsRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseOutputCache();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.MapGet("/api/ProhibitedWords", async (IProhibitedWordsService prohibitedWords) =>
//{
//    var t = await prohibitedWords.GetAllAsync();
//    return t.Data;
//}).CacheOutput();

app.Run();
