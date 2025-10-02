using ShAbedi.PayaSystem.Domain.Base;
using ShAbedi.PayaSystem.Domain.Enums;

namespace ShAbedi.PayaSystem.Domain.Entities;
public class Transaction: BaseEntity
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; }

    public long Amount { get; private set; } // Negative for debit, positive for credit
    public TransactionType Type { get; private set; }
    public string? Note { get; private set; }


    public static Transaction Create(Guid id, Guid accountId, long amount, TransactionType type, string? note)
    {
        return new Transaction()
        {
            Id = id,
            AccountId = accountId,
            Amount = amount,
            Type = type,
            Note = note,
        };
    }
}