using AutoMapper;
using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CancelShebaCommand;

[AutoMap(typeof(FinalizeShebaCommandRequest))]

public record CancelShebaCommand(string Note) : IRequest<ShebaCommandResponse>
{
    public Guid RequestId { get; set; }
};