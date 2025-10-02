using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;
using ShAbedi.PayaSystem.Domain.Entities;

namespace ShAbedi.PayaSystem.Application.Common.Contracts;

public interface IShebaQueryRepository
{
    Task<ShebaQueryResponse> Get(int pageSize, int pageNumber, CancellationToken cancellationToken);
    Task<ShebaRequest?> FindById(Guid id, CancellationToken cancellationToken);
   
    Task<List<ShebaRequest>> FindReadyToComplete(int pageSize, int pageNumber, CancellationToken cancellationToken);
    Task<int> CountReadyToComplete(CancellationToken cancellationToken);
    
    Task<List<ShebaRequest>> FindReadyToCancel(int pageSize, int pageNumber, CancellationToken cancellationToken);
    Task<int> CountReadyToCancel(CancellationToken cancellationToken);

    Task<List<ShebaRequest>> FindReadyToRetry(int pageSize, int pageNumber, int retryLimit, CancellationToken cancellationToken);
    Task<int> CountReadyToRetry(int retryLimit, CancellationToken cancellationToken);
}
