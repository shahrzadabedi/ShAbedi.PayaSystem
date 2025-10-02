using ShAbedi.PayaSystem.Domain.Contracts;

namespace ShAbedi.PayaSystem.Api.Extensions;
public static class DatabaseInitializerRegistrar
{
    public static async Task InitializeDatabasesAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>().InitializeDatabaseAsync();
    }
}
