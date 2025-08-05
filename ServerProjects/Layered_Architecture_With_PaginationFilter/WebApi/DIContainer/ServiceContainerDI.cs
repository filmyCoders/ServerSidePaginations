using Business.Contract.IServices;
using Business.Contract.Services;
using DataAccess.Db_Context;
using DataAccess.Repositories.IRepo;
using DataAccess.Repositories.Repo;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DIContainer
{
    public static class ServiceContainerDI
    {

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddDbContext<AppDbContext>(option => option

            .UseSqlServer(configuration.GetConnectionString("Databasestring")));

            services.AddScoped<ICarRepo, CarRepo>();
             services.AddScoped<ICarService, CarService>();
            return services;

        }
    }
}
