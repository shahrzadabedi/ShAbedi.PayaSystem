using AutoMapper;
using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Queries;

[AutoMap(typeof(ShebaQueryRequest))]
public record ShebaQuery(int PageSize, int PageNumber): IRequest<ShebaQueryResponse>;

