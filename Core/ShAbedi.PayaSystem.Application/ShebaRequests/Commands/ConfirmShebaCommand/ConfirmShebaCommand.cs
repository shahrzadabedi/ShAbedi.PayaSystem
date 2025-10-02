using AutoMapper;
using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.FinalizeShebaCommand;

[AutoMap(typeof(FinalizeShebaCommandRequest))]

public record ConfirmShebaCommand(): IRequest<ShebaCommandResponse>
{
    public Guid RequestId { get; set; }
};