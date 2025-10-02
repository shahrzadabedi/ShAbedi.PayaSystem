namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

public class ShebaRequestModel
{
    public Guid Id { get; set; }
    public long Price { get; set; }
    public string FromShebaNumber { get; set; }
    public string ToShebaNumber { get; set; }
    public string Status { get; set; }
    public string CreatedAt { get; set; }
}
