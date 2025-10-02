using ShAbedi.PayaSystem.Domain.Entities;

namespace ShAbedi.PayaSystem.Application.Common.Contracts;

public interface IAccountQueryRepository
{
    Task<Account?> FindByShebaNumber(string shebaNumber, CancellationToken cancellationToken);
}
