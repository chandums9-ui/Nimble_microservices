using CoreAccounting.Infra;
using CoreAccounting.App;
using Common.API;
using Common.API.ExceptionHandling;
using Common.API.Authorization;
using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
//using AutoMapper;
using Microsoft.AspNetCore.Mvc;
//using CoreAccounting.API.Middlewares.ActionFilters;
using CoreAccounting.App.Contracts;

using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.Extensions.Configuration;
//using CoreAccounting.API.Middlewares.Authorization;
//using CoreAccounting.API.Middlewares.ExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Common.Infra;
using Common.App.Contracts;
using Common.Infra.Logger;
using NLog;
using CoreAccounting.Domain.DTO.Model;
using Microsoft.AspNetCore.Http.Features;
using Common.Domain.DTO.Resp;
using Common.Domain.DTO.Model;
using CoreAccounting.API.Hubs;
using Messages.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly().);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("NimbleProperty.CoreAccounting.Domain"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonInfraServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddSignalR();

// Add email services to the container.
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

var RedisCacheConfig = builder.Configuration
        .GetSection("RedisCacheConfiguration")
        .Get<RedisCacheSettings>();
builder.Services.AddSingleton(RedisCacheConfig);

var RedisCacheApproval = builder.Configuration
        .GetSection("RedisCacheApproval")
        .Get<RedisCacheApprovalDTO>();
builder.Services.AddSingleton(RedisCacheApproval);
builder.Services.Configure<EnvironmentWiseCacheKey>(builder.Configuration.GetSection("EnvironmentWiseCacheKey"));
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddAuthInfraServices(builder.Configuration); 
builder.Services.AddCoreApplicationServices(builder.Configuration);
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Nimble Property OpenApi", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors();

var app = builder.Build();
var cultureInfo = new CultureInfo("en-US");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthJwtMiddleware>();
app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader());
app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();
app.MapHub<SignalRHub>("/feedHub");
app.Run();
