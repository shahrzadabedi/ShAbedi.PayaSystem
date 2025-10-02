using System.Data;
using MediatR;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.Exceptions;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.FinalizeShebaCommand;

public class ConfirmShebaCommandHandler(
    IShebaQueryRepository query,
    IShebaCommandRepository command,
    IUnitOfWork unitOfWork) : IRequestHandler<ConfirmShebaCommand, ShebaCommandResponse>
{
    public async Task<ShebaCommandResponse> Handle(ConfirmShebaCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        var shebaRequest = await query.FindById(request.RequestId, cancellationToken);
        if (shebaRequest == null)
            throw new ShebaRequestNotFoundException();
        try
        {
            shebaRequest.SetAsReadyToComplete();

            await unitOfWork.CommitAsync(cancellationToken);

            return new ShebaCommandResponse(new ShebaRequestModel()
            {
                CreatedAt = shebaRequest.CreatedAt.ToString(),
                FromShebaNumber = shebaRequest.FromShebaNumber,
                ToShebaNumber = shebaRequest.ToShebaNumber,
                Id = shebaRequest.Id,
                Status = "confirmed",
                Price = shebaRequest.Price
            }, "Request is Confirmed");
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
