using MediatR;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Queries;

public class ShebaQueryHandler(IShebaQueryRepository query) : IRequestHandler<ShebaQuery, ShebaQueryResponse>
{
    public async Task<ShebaQueryResponse> Handle(ShebaQuery request, CancellationToken cancellationToken)
    {
        return await query.Get(request.PageSize, request.PageNumber, cancellationToken);
    }
}
