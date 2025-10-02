using Microsoft.EntityFrameworkCore;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Domain.Entities;
using ShAbedi.PayaSystem.Infrastructure.Persistence;

namespace ShAbedi.PayaSystem.Infrastructure.Repositories;

public class AccountQueryRepository : IAccountQueryRepository
{
    private readonly AppDbContext _context;
    public AccountQueryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> FindByShebaNumber(string shebaNumber, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts
            .Include(a => a.AmountLocks)
            .Include(p=> p.Transactions)
            .FirstOrDefaultAsync(ac => ac.ShebaNumber == shebaNumber,cancellationToken );
        
        return account;
    }
}
