namespace ShAbedi.PayaSystem.Domain.Enums;

public enum AmountLockStatus
{
    Locked = 0,       // Locked 
    Released = 1,     // Lock Release (Transfer was successful)
    Canceled = 2,     // Transfer was canceled
    Failed = 3        // Transfer failed
}

