using Microsoft.EntityFrameworkCore;

namespace TaskBoard.Framework.Core.Utils.EntityFrameworkCore;

public class DatabasePreSeederPostCleaner<TDbContext> : IDisposable
    where TDbContext : DbContext
{
    protected readonly TDbContext _context;

    public DatabasePreSeederPostCleaner(TDbContext context)
    {
        _context = context;
    }

    protected virtual void DisableForeignKeys()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                #pragma warning disable EF1002
                _context.Database.ExecuteSqlRaw($"ALTER TABLE [{schema}].[{tableName}] NOCHECK CONSTRAINT ALL;");
                #pragma warning restore EF1002
            }
        }
    }

    protected virtual void EnableForeignKeys()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                #pragma warning disable EF1002
                _context.Database.ExecuteSqlRaw($"ALTER TABLE [{schema}].[{tableName}] WITH CHECK CHECK CONSTRAINT ALL;");
                #pragma warning restore EF1002
            }
        }
    }

    public virtual void PopulateDatabase(object[] data)
    {
        _context.Database.EnsureCreated();
        DisableForeignKeys();

        foreach (object entity in data)
        {
            _context.Add(entity);
        }
        _context.SaveChanges();

        EnableForeignKeys();
    }

    protected virtual void ClearDatabase()
    {
        var entityTypes = _context.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var tableName = entityType.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                var schema = entityType.GetSchema() ?? "dbo";
                #pragma warning disable EF1002
                _context.Database.ExecuteSqlRaw($"DELETE FROM [{schema}].[{tableName}];");
                #pragma warning restore EF1002
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DisableForeignKeys();

        ClearDatabase();

        EnableForeignKeys();
    }
}