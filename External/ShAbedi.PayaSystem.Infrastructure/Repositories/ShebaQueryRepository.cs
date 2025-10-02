using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;
using ShAbedi.PayaSystem.Domain.Entities;
using ShAbedi.PayaSystem.Domain.Enums;
using ShAbedi.PayaSystem.Infrastructure.Persistence;

namespace ShAbedi.PayaSystem.Infrastructure.Repositories;

public class ShebaQueryRepository(AppDbContext context, IMapper mapper) : IShebaQueryRepository
{
    public async Task<ShebaQueryResponse> Get(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.Pending); 
        
        var count = query.Count();
       
        var result = await query
        .OrderBy(p => p.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(p=> new ShebaQueryResponseModel()
        {
            Id = p.Id,
            Status = p.Status.ToString(),
            CreatedAt = p.CreatedAt,
            FromShebaNumber = p.FromShebaNumber,
            ToShebaNumber = p.ToShebaNumber,
            Price = p.Price
        })
        .ToListAsync(cancellationToken);

        return new ShebaQueryResponse( result , count);
    }

    public async Task<ShebaRequest?> FindById(Guid id, CancellationToken cancellationToken)
    {
        return await context.ShebaRequest.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<ShebaRequest>> FindReadyToComplete(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyToComplete);

        var result = await query.OrderBy(p=> p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<int> CountReadyToComplete(CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyToComplete);
        
        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<ShebaRequest>> FindReadyToCancel(int pageSize, int pageNumber, CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyToCancel);

        var result = await query.OrderBy(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<int> CountReadyToCancel(CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyToCancel);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<ShebaRequest>> FindReadyToRetry(int pageSize, int pageNumber, int retryLimit, CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyForRetry && p.RetryCount<=retryLimit);

        var result = await query.OrderBy(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return result;
    }
    public async Task<int> CountReadyToRetry(int retryLimit, CancellationToken cancellationToken)
    {
        var query = context.ShebaRequest.Where(p => p.Status == ShebaRequestStatus.ReadyForRetry && p.RetryCount <= retryLimit);

        return await query.CountAsync(cancellationToken);
    }
}
