namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

public class ShebaQueryResponse
{
    public ShebaQueryResponse(List<ShebaQueryResponseModel> requests, int totalCount)
    {
        Requests = requests;
        TotalCount = totalCount;
    }

    public List<ShebaQueryResponseModel> Requests { get; private set; }
    public int TotalCount { get; private set; }
}

public class ShebaQueryResponseModel
{
    public Guid Id { get; set; }
    public long Price { get; set; }
    public string Status { get; set; }
    public string FromShebaNumber { get; set; }
    public string ToShebaNumber { get; set; }
    public DateTime CreatedAt { get; set; }
}
