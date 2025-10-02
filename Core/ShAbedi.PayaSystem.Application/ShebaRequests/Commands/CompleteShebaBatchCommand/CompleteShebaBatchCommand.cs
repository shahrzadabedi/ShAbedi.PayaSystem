using MediatR;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CompleteShebasCommand;

public record CompleteShebaBatchCommand(): IRequest<bool>;
