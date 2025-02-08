using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;
using Xunit;

namespace TaskBoard.Framework.Core.Utils.Testcontainers;

public class ContainerizedSqlServerFixture : IContainerizedDatabaseFixture, IAsyncLifetime
{
    private readonly MsSqlContainer container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .Build();

    public IDatabaseContainer Container => container;

    public string ConnectionString => container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}
