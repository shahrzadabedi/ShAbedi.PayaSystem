namespace ShAbedi.PayaSystem.Domain.Enums;

public enum TransactionType
{
    Lock = 0,       // Lock
    Debit = 1,      // withdraw
    Credit = 2,     // deposit
    CancelLock = 3, // Cancel Lock
    ReleaseLock = 4,//Release Lock
    FailLock = 5   // Fail Lock
}
