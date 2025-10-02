using ShAbedi.PayaSystem.Domain.Base;
using ShAbedi.PayaSystem.Domain.Enums;

namespace ShAbedi.PayaSystem.Domain.Entities;

public class ShebaRequest : BaseEntity
{
    private readonly List<AmountLock> _amountLocks = new();

    public Guid Id { get; private set; }
    public long Price { get; private set; }
    public ShebaRequestStatus Status { get; private set; } = ShebaRequestStatus.Pending;
    public string FromShebaNumber { get; private set; } // IR + 24 digits
    public string ToShebaNumber { get; private set; }  // IR + 24 digits
    public string? Note { get; private set; }

    public Guid? FromAccountId { get; private set; }
    public Account? FromAccount { get; private set; }

    public DateTime? ReadyToCompleteDateTime { get; private set; }
    public DateTime? CompleteDateTime { get; private set; }
    public DateTime? CancelDateTime { get; private set; }
    public DateTime? ReadyToCancelDateTime { get; private set; }
    
    public DateTime? ReadyToRetryDateTime { get; private set; }
    public int RetryCount { get; private set; } = 0;
    
    public DateTime? FailedDateTime { get; private set; }
    public IReadOnlyCollection<AmountLock> AmountLocks => _amountLocks;

    public static ShebaRequest Create(Guid id, long price, string fromShebaNumber, string toShebaNumber, string? note, Guid fromAccountId)
    {
        return new ShebaRequest()
        {
            Id = id,
            Price = price,
            Status = ShebaRequestStatus.Pending,
            FromAccountId = fromAccountId,
            FromShebaNumber = fromShebaNumber,
            ToShebaNumber = toShebaNumber,
            Note = note,
            CreatedAt = DateTime.Now
        };
    }

    public void SetAsReadyToComplete()
    {
        Status = ShebaRequestStatus.ReadyToComplete;
        ReadyToCompleteDateTime = DateTime.Now;
    }

    public void SetAsCompleted()
    {
        Status = ShebaRequestStatus.Completed;
        CompleteDateTime = DateTime.Now;
    }

    public void SetAsCanceled()
    {
        Status = ShebaRequestStatus.Canceled;
        CancelDateTime = DateTime.Now;
    }

    public void SetAsReadyToRetry()
    {
        Status = ShebaRequestStatus.ReadyForRetry;
        ReadyToRetryDateTime = DateTime.Now;
    }

    public void SetAsReadyToCancel()
    {
        Status = ShebaRequestStatus.ReadyToCancel;
        ReadyToCancelDateTime = DateTime.Now;
    }

    public void SetAsFailed()
    {
        Status = ShebaRequestStatus.Failed;
        FailedDateTime = DateTime.Now;
    }

    public void IncreaseRetryCount() => RetryCount++;
}
