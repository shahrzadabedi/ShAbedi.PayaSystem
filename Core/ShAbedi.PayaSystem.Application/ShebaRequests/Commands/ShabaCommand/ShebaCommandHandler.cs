using System.Data;
using MediatR;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.Exceptions;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;
using ShAbedi.PayaSystem.Domain.Entities;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;

public class ShebaCommandHandler(
    IShebaCommandRepository shebaCommand,
    IAccountQueryRepository accountQuery,
    IAccountCommandRepository accountCommand,
    IUnitOfWork unitOfWork) : IRequestHandler<ShebaCommand, ShebaCommandResponse>
{
    public async Task<ShebaCommandResponse> Handle(ShebaCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            var userAccount = await accountQuery.FindByShebaNumber(request.FromShebaNumber, cancellationToken);
            if (userAccount == null)
                throw new AccountNotFoundException();

            var shebaRequestId = request.ShebaRequestId;
            var shebaRequest = ShebaRequest.Create(shebaRequestId,
                request.Price,
                request.FromShebaNumber,
                request.ToShebaNumber,
                request.Note,
                userAccount.Id);
            shebaCommand.Add(shebaRequest);

            userAccount.LockAmount(request.Price, shebaRequestId, request.Note);

            accountCommand.UpdateAccount(userAccount);

            await unitOfWork.CommitAsync(cancellationToken);

            return new ShebaCommandResponse(new ShebaRequestModel()
            {
                FromShebaNumber = request.FromShebaNumber,
                ToShebaNumber = request.ToShebaNumber,
                CreatedAt = shebaRequest.CreatedAt.ToString(),
                Id = shebaRequest.Id,
                Price = request.Price,
                Status = shebaRequest.Status.ToString()
            }, "Request is saved successfully and is in pending status");
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
