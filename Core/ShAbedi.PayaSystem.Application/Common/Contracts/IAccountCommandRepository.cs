using ShAbedi.PayaSystem.Domain.Entities;

namespace ShAbedi.PayaSystem.Application.Common.Contracts;

public interface IAccountCommandRepository
{
    void UpdateAccount(Account account);
    void UpdateAccountComplete(Account account);
}
