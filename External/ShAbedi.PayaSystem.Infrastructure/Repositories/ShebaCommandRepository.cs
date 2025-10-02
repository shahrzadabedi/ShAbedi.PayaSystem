using Microsoft.EntityFrameworkCore;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Domain.Entities;
using ShAbedi.PayaSystem.Infrastructure.Persistence;

namespace ShAbedi.PayaSystem.Infrastructure.Repositories;

public class ShebaCommandRepository(AppDbContext context) : IShebaCommandRepository
{
    public void Add(ShebaRequest request)
    {
        context.ShebaRequest.Add(request);
    }

    public void Update(ShebaRequest request)
    {
        context.Entry(request).State = EntityState.Modified;
    }
}
