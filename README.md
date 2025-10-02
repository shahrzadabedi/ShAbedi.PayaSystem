# ShAbedi.PayaSystem
This project Has been designed to simulate Paya System.
It consist two services: 1: ShAbedi.PayaSystem.Api 2: ShAbedi.PayaSystem.Jobs
The architecture is clean architecture.
PayaRequest can have different States:

public enum ShebaRequestStatus
{
    Pending= 0,
    ReadyToComplete = 1, // The same as Confirmed
    ReadyForRetry = 2,
    Completed = 3,
    ReadyToCancel = 4,
    Canceled = 5,
    Failed = 6
}
Workflow:
When a user places a ShebaCommand, it is now the operator's turn either to Confirm or Cancel the Sheba request. After confirming it the request is schduled to transfer the money to destination account. If the request is canceled by the operator it is then scheduled to return to the account owner. 

Locking the Amount:
We lock the amount of money for the user to later transfer to the destination account.

Lock States:
public enum AmountLockStatus
{
    Locked = 0,       // Locked 
    Released = 1,     // Lock Release (Transfer was successful)
    Canceled = 2,     // Transfer was canceled
    Failed = 3        // Transfer failed
}

Transaction Types:
public enum TransactionType
{
    Lock = 0,       // Lock
    Debit = 1,      // withdraw
    Credit = 2,     // deposit
    CancelLock = 3, // Cancel Lock
    ReleaseLock = 4,//Release Lock
    FailLock = 5   // Fail Lock
}
