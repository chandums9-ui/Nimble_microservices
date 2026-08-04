using Common.App.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Infra.AuthDBCon;
using Common.Infra.DataRepos;
using Common.App.Services;
namespace Common.API
{
    public static class AuthInfraDependencyInjection
    {
        public static IServiceCollection AddAuthInfraServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddScoped<IAuthUnitOfWork, AuthUnitOfwork>();
            services.AddScoped<IAuthJwtValidation, OpenAPIAuthValidation>();
            services.AddDbContext<AuthDBContext>(options => options.UseSqlServer(config.GetConnectionString("AuthDBConnection")));
            return services;
        }
    }
}
