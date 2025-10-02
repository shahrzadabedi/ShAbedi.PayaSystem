using ShAbedi.PayaSystem.Domain.Base;
using ShAbedi.PayaSystem.Domain.Enums;
using ShAbedi.PayaSystem.Domain.Exceptions;

namespace ShAbedi.PayaSystem.Domain.Entities;

public class Account : BaseEntity
{
    public Guid Id { get; private set; }

    //private readonly List<Transaction> _transactions = new();
    //private readonly List<AmountLock> _amountLocks = new();

    public string OwnerName { get; private set; } = string.Empty;
    public string ShebaNumber { get; private set; } = string.Empty; // Format: IR + 24 digits
    public long Balance { get; private set; } // In Rials

    public DateTime? ModifiedAt { get; private set; }
    public List<Transaction> Transactions { get; private set; } =new();
    public List<AmountLock> AmountLocks { get; private set; }= new();

    public void LockAmount(long amount,Guid shebaRequestId, string? note = null)
    {
        if (amount <= 0)
            throw new ValidationException("مبلغ مورد نظر باید مثبت باشد","Amount_Only_Positive");

        if ((Balance + AmountLocks.Sum(p=> p.Amount)< amount))
            throw new InsufficientBalanceException();
        

        var amountLock = AmountLock.Create(-amount, shebaRequestId, Id);
        AmountLocks.Add(amountLock);

        var transaction = Transaction.Create(Guid.NewGuid(), Id, -amount, TransactionType.Lock, note);

        Transactions.Add(transaction);
        ModifiedAt = DateTime.Now;
    }

    public void WithdrawLockedAmount(string? note, Guid shebaRequestId)
    {
        var amountLock = AmountLocks.FirstOrDefault(p => p.ShebaRequestId == shebaRequestId);
        var lockedAmount = amountLock?.Amount;

        if (lockedAmount != null)
        {
            Balance += lockedAmount.Value;

            var transactionReleaseLock = Transaction.Create(Guid.NewGuid(), Id, lockedAmount.Value, TransactionType.ReleaseLock, note);
            Transactions.Add(transactionReleaseLock);
            
            var transactionDebit = Transaction.Create(Guid.NewGuid(), Id, lockedAmount.Value, TransactionType.Debit, note);
            Transactions.Add(transactionDebit);

            ModifiedAt = DateTime.Now;
            amountLock!.SetReleased();
        }
    }

    public void CancelLock(string note, Guid shebaRequestId)
    {
        var amountLock = AmountLocks.FirstOrDefault(p => p.ShebaRequestId == shebaRequestId);
        var lockedAmount = amountLock?.Amount;
        if (amountLock != null)
        {
            amountLock.SetCanceled();
            var transaction = Transaction.Create(Guid.NewGuid(), Id, -lockedAmount.Value, TransactionType.CancelLock, note);

            Transactions.Add(transaction);
            ModifiedAt = DateTime.Now;
        }
    }

    public void Deposit(string? note, long amount)
    {
        if (amount <= 0)
            throw new ValidationException("مبلغ مورد نظر باید مثبت باشد", "Amount_Only_Positive");

        Balance += amount;
        
        var transaction = Transaction.Create(Guid.NewGuid(), Id, amount, TransactionType.Credit, note);

        Transactions.Add(transaction);
        ModifiedAt = DateTime.Now;
    }

    public void FailLock(string note, Guid shebaRequestId)
    {
        var amountLock = AmountLocks.FirstOrDefault(p => p.ShebaRequestId == shebaRequestId);
        var lockedAmount = amountLock?.Amount;
        if (amountLock != null)
        {
            amountLock.SetFailed();
            var transaction = Transaction.Create(Guid.NewGuid(), Id, -lockedAmount.Value, TransactionType.FailLock, note);

            Transactions.Add(transaction);
            ModifiedAt = DateTime.Now;
        }
    }
}
