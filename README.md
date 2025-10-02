# ShAbedi.PayaSystem

This project has been designed to simulate the **Paya System**.

It consists of two services:
1. **ShAbedi.PayaSystem.Api**
2. **ShAbedi.PayaSystem.Jobs**

The architecture follows the **Clean Architecture** pattern.

---

## 🧾 Sheba Request States

```csharp
public enum ShebaRequestStatus
{
    Pending = 0,
    ReadyToComplete = 1, // The same as Confirmed
    ReadyForRetry = 2,
    Completed = 3,
    ReadyToCancel = 4,
    Canceled = 5,
    Failed = 6
}
```
### Workflow

When a user places a ShebaCommand, it becomes the operator's responsibility to either Confirm or Cancel the Sheba request.

If Confirmed, the request is scheduled to transfer the money to the destination account.

If Canceled, it is scheduled to return the amount to the account owner.

## 💰 Locking the Amount

We **lock the amount** of money for the user to later transfer it to the destination account.

To achieve this, the system uses the **AmountLocks** table.  
Each entry in this table represents a reserved portion of money that cannot be used until the corresponding transfer is either completed, canceled, or failed.

The **AmountLocks** table stores the lock status to indicate the current state of the reserved funds:

- When a request is created, an **AmountLock** record is added with the status **Locked**.  
- Once the transfer completes successfully, the status changes to **Released**.  
- If the transfer is canceled, the status changes to **Canceled**, and the locked amount becomes available again.  
- In case of a failed transfer attempt, the status changes to **Failed**, signaling that the operation didn’t succeed and needs retry or investigation.

Lock States:
```csharp
public enum AmountLockStatus
{
    Locked = 0,       // Locked 
    Released = 1,     // Lock Release (Transfer was successful)
    Canceled = 2,     // Transfer was canceled
    Failed = 3        // Transfer failed
}
```
Transaction Types:
```csharp
public enum TransactionType
{
    Lock = 0,       // Lock
    Debit = 1,      // withdraw
    Credit = 2,     // deposit
    CancelLock = 3, // Cancel Lock
    ReleaseLock = 4,//Release Lock
    FailLock = 5   // Fail Lock
}
```
