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
### **Workflow**

The workflow of the system is designed to simulate how a real Paya request is processed from creation to completion.

1. A **user** initiates a transfer by creating a **ShebaCommand**.  
2. The **operator** reviews the request and decides whether to **Confirm** or **Cancel** it.  
3. If the request is **Confirmed**, it moves to the **ReadyToComplete** state and is scheduled for fund transfer to the destination account.  
4. If the request is **Canceled**, it moves to the **ReadyToCancel** state and is scheduled to return the locked amount to the account owner.  
5. During execution, the system locks the required amount using the **AmountLocks** table to ensure that the funds are reserved for the operation.  
6. Once the transfer or refund is processed:
   - If successful → The request state changes to **Completed**, and the lock status changes to **Released**.  
   - If canceled → The lock status becomes **Canceled**.  
   - If failed → The system updates both the request and lock to **Failed**, and the operation can be retried if needed.


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
