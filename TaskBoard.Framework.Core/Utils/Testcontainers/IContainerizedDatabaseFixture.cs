using DotNet.Testcontainers.Containers;

namespace TaskBoard.Framework.Core.Utils.Testcontainers;

public interface IContainerizedDatabaseFixture
{
    IDatabaseContainer Container { get; }
    string ConnectionString { get; }
}
