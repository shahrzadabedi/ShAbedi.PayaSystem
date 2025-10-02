namespace ShAbedi.PayaSystem.Domain.Enums;

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
