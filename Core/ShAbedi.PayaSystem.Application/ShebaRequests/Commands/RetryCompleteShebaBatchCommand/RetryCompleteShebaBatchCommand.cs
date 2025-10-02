using MediatR;

namespace 
ShAbedi.PayaSystem.Application.ShebaRequests.Commands.RetryCompleteShebaBatchCommand;

public record RetryCompleteShebaBatchCommand : IRequest<bool>;