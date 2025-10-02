using Microsoft.EntityFrameworkCore;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Domain.Entities;
using ShAbedi.PayaSystem.Infrastructure.Persistence;

namespace ShAbedi.PayaSystem.Infrastructure.Repositories;

public class AccountCommandRepository(AppDbContext context) : IAccountCommandRepository
{
    public void UpdateAccount(Account account)
    {
        foreach (var lockItem in account.AmountLocks)
        {
            if (context.Entry(lockItem).State == EntityState.Modified || context.Entry(lockItem).State == EntityState.Detached)
                context.Entry(lockItem).State = EntityState.Added;
        }

        foreach (var transaction in account.Transactions)
        {
            if (context.Entry(transaction).State == EntityState.Modified || context.Entry(transaction).State == EntityState.Detached)
                context.Entry(transaction).State = EntityState.Added;
        }

        context.Entry(account).State = EntityState.Modified;
    }

    public void UpdateAccountComplete(Account account)
    {
        foreach (var lockItem in account.AmountLocks)
        {
            if (context.Entry(lockItem).State == EntityState.Modified || context.Entry(lockItem).State == EntityState.Detached)
                context.Entry(lockItem).State = EntityState.Modified;
        }

        foreach (var transaction in account.Transactions)
        {
            if (context.Entry(transaction).State == EntityState.Modified || context.Entry(transaction).State == EntityState.Detached)
                context.Entry(transaction).State = EntityState.Added;
        }

        context.Entry(account).State = EntityState.Modified;
    }
}
