using ShAbedi.PayaSystem.Domain.Base;
using ShAbedi.PayaSystem.Domain.Enums;

namespace ShAbedi.PayaSystem.Domain.Entities;

public class AmountLock : BaseEntity
{
    public Guid Id { get; private set; }
    public AmountLockStatus Status { get; private set; }
    public Guid AccountId { get;  set; }
    public Account Account { get; set; }

    public long Amount { get; private set; } // Should be Negative for Lock
    public Guid ShebaRequestId { get; set; }
    public ShebaRequest ShebaRequest { get; set; }

    public static AmountLock Create(long amount, Guid shebaRequestId, Guid accountId)
    {
        return new AmountLock()
        {
            Id = Guid.NewGuid(),
            Amount = amount,
            Status = AmountLockStatus.Locked,
            AccountId = accountId,
            ShebaRequestId = shebaRequestId
        };
    }

    public void SetReleased() => Status = AmountLockStatus.Released;

    public void SetCanceled() => Status = AmountLockStatus.Canceled;

    public void SetFailed() => Status = AmountLockStatus.Failed;
}
