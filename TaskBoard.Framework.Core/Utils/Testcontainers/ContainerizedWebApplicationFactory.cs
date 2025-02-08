using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaskBoard.Framework.Core.Utils.Testcontainers;

public class ContainerizedWebApplicationFactory<TStartup, TDbContext> : WebApplicationFactory<TStartup>
    where TStartup : class
    where TDbContext : DbContext
{
    private readonly IContainerizedDatabaseFixture _databaseClassFixture;
    private IServiceScope? _serviceScope;

    public ContainerizedWebApplicationFactory(IContainerizedDatabaseFixture databaseClassFixture)
    {
        _databaseClassFixture = databaseClassFixture;
        _serviceScope = null;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptors = services.Where(d => d.ServiceType == typeof(TDbContext) || d.ServiceType == typeof(DbContextOptions<TDbContext>)).ToList();
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TDbContext>(options => 
                options.UseSqlServer(_databaseClassFixture.ConnectionString));
        });
    }

    public TService GetService<TService>()
    {
        _serviceScope ??= Services.CreateScope();

        return (TService)_serviceScope.ServiceProvider.GetRequiredService(typeof(TService)) ??
            throw new NullReferenceException($"{typeof(TService)} service could not be found");
    }

    protected override void Dispose(bool disposing)
    {
        _serviceScope?.Dispose();
    }
}
