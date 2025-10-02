namespace ShAbedi.PayaSystem.Domain.Base;

public class BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;
}