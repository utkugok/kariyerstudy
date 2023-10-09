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
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<JobService>();
builder.Services.AddScoped<JobRepository>();


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

app.Run();
