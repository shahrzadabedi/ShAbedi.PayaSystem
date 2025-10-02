using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShAbedi.PayaSystem.Domain.Contracts;

namespace ShAbedi.PayaSystem.Infrastructure.Persistence.Initialization;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly AppDbContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(AppDbContext context, ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_context.Database.GetMigrations().Any())
                return;

            if (!(await _context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                return;

            await _context.Database.MigrateAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            throw;
        }
    }
}
