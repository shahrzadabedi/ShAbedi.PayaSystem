using AutoMapper;
using ShAbedi.PayaSystem.Application.ShebaRequests.Queries;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

[AutoMap(typeof(ShebaQuery))]
public class ShebaQueryRequest
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
