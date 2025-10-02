namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

public class ShebaCommandResponse
{
    public ShebaCommandResponse(ShebaRequestModel request, string message)
    {
        Request = request;
        Message = message;
    }

    public ShebaRequestModel Request { get; private set; }
    public string Message { get; private set; }
}
