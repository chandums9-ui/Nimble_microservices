using Common.API;
using Common.API.Authorization;
using Common.API.ExceptionHandling;
using Common.App.Contracts;
using Common.Infra;
using Common.Infra.Logger;
using Dashboard.Analytics.Infra;
using Dashboard.App;
using Dashboard.Infra;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddAnalyticsInfraServices(builder.Configuration);
builder.Services.AddCommonInfraServices(builder.Configuration);
builder.Services.AddInfraServices(builder.Configuration);
builder.Services.AddAuthInfraServices(builder.Configuration);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//FlurlHttp.Configure(settings =>
//{
//    var jsonSettings = new JsonSerializerSettings
//    {
//        NullValueHandling = NullValueHandling.Ignore,
//        ObjectCreationHandling = ObjectCreationHandling.Replace,
//        ContractResolver = new CamelCasePropertyNamesContractResolver()
//    };
//    settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
//});
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Nimble Property Dashboard API", Version = "v1" });
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

//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    string[] supportedCultures = new[] { "en-US" };
//    options
//        .AddSupportedCultures(supportedCultures)
//        .AddSupportedUICultures(supportedCultures)
//        .SetDefaultCulture("en-US");
//});

var cultureInfo = new CultureInfo(builder.Configuration.GetSection("DefaultCulture").Value);
var app = builder.Build();

// Configure the HTTP request pipeline.

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

app.Run();
