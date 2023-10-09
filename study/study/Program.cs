using study.Extensions;
using study.Repositories;
using study.Repositories.Interfaces;
using study.Services;
using study.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElastic(builder.Configuration);

builder.Services.AddOutputCache();
builder.Services.AddScoped<ICacheService,CacheService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IProhibitedWordsService, ProhibitedWordsService>();
builder.Services.AddScoped<IProhibitedWordsRepository, ProhibitedWordsRepository>();


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
app.UseOutputCache();

app.MapGet("/api/ProhibitedWords", async (context) =>
{
}).CacheOutput();

app.Run();
