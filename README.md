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

### 💰 Locking the Amount

We lock the amount of money for the user to later transfer it to the destination account.

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
