using MediatR;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.Exceptions;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CancelShebaCommand;

public class CancelShebaCommandHandler(
    IShebaQueryRepository query,
    IShebaCommandRepository command,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelShebaCommand, ShebaCommandResponse>
{
    public async Task<ShebaCommandResponse> Handle(CancelShebaCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        var shebaRequest = await query.FindById(request.RequestId, cancellationToken);
        if (shebaRequest == null)
            throw new ShebaRequestNotFoundException();
        try
        {
            shebaRequest.SetAsReadyToCancel();
            command.Update(shebaRequest);

            await unitOfWork.CommitAsync(cancellationToken);

            return new ShebaCommandResponse(new ShebaRequestModel()
            {
                CreatedAt = shebaRequest.CreatedAt.ToString(),
                FromShebaNumber = shebaRequest.FromShebaNumber,
                ToShebaNumber = shebaRequest.ToShebaNumber,
                Id = shebaRequest.Id,
                Status = "canceled",
                Price = shebaRequest.Price
            }, "Request is Canceled");
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

