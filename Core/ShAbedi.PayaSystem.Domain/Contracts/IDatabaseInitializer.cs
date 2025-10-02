namespace ShAbedi.PayaSystem.Domain.Contracts;

public interface IDatabaseInitializer
{
    Task InitializeDatabaseAsync(CancellationToken cancellationToken = default);
}