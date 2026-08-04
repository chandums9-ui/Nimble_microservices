//using Payable.Infra;
using Payable.App;

using FluentValidation.AspNetCore;
using System.Reflection;
using FluentValidation;
//using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Common.API.ActionFilters;
using Payable.App.Contracts;
using Payable.Infra;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Common.API.Authorization;
using Common.API.ExceptionHandling;
using Microsoft.AspNetCore.Builder;
using Common.API;
using Common.Infra;
using NLog;
using Common.App.Contracts;
using Common.Infra.Logger;
using Common.Domain.DTO.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Payable.Infra.PaymentGateWay;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly().);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("NimbleProperty.Payable.Domain"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Nimble Property PayableApi", Version = "v1" });
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
            Array.Empty<string>()
        }
    });

    // ? Include XML comments for Swagger UI to display method summaries/descriptions
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    option.IncludeXmlComments(xmlPath);
});

builder.Services.AddCommonInfraServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddAuthInfraServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));
builder.Services.AddSingleton<ILoggerService, LoggerService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.Configure<PaymentGateWayDetails>(builder.Configuration.GetSection("PaymentGateWayInfo"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression();


builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
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
app.UseResponseCompression();
app.Run();
