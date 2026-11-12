using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Internal;

namespace BarberBoss.Infrastructure;

public static class DependencyInjectionExtensions
{
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
        services.AddDbContext<BarberBossDbContext>(options =>
            options.UseMySql(
                connectionString, 
                serverVersion,
                mySqlOptions => mySqlOptions.EnableRetryOnFailure()));

    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IReadOnlyRepository<Billing>, BillingRepository>();
        services.AddScoped<IUpdateOnlyRepository<Billing>, BillingRepository>();
        services.AddScoped<IWriteOnlyRepository<Billing>, BillingRepository>();
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddUnitOfWork(services);
        AddRepositories(services);
    }
}
