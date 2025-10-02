using AutoMapper;
using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;

[AutoMap(typeof(ShebaCommandRequest))]
public record ShebaCommand
    (long Price, string FromShebaNumber, string ToShebaNumber, string? Note) : IRequest<ShebaCommandResponse>
{
    public Guid  ShebaRequestId { get; set; }
};
