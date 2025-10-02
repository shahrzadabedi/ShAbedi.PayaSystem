using System.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShAbedi.PayaSystem.Application.Common.Contracts;
namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CompleteShebasCommand;

internal class CompleteShebaBatchCommandHandler(
    IShebaQueryRepository query,
    IShebaCommandRepository command,
    IAccountQueryRepository accountQuery,
    IAccountCommandRepository accountCommand,
    IUnitOfWork unitOfWork,
    ILogger<CompleteShebaBatchCommandHandler> logger) : IRequestHandler<CompleteShebaBatchCommand, bool>
{
    public async Task<bool> Handle(CompleteShebaBatchCommand request, CancellationToken cancellationToken)
    {
        var count = await query.CountReadyToComplete(cancellationToken);

        var batchSize = 10;
        int iterationsCount = (int)Math.Ceiling(((float)(count / batchSize)));

        for (int i = 0; i <= iterationsCount; i++)
        {
            var shebaRequests = await query.FindReadyToComplete(batchSize, i + 1, cancellationToken);

            foreach (var shebaRequest in shebaRequests)
            {
                var enrichers = new List<IDisposable>()
                {
                    LogContext.PushProperty("RequestId", shebaRequest.Id.ToString())
                };

                using (new DisposableEnricherScope(enrichers));

                await unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
                try
                {
                    var fromAccount =
                        await accountQuery.FindByShebaNumber(shebaRequest.FromShebaNumber, cancellationToken);
                    var toAccount =
                        await accountQuery.FindByShebaNumber(shebaRequest.ToShebaNumber, cancellationToken);

                    if (fromAccount == null)
                    {
                        shebaRequest.SetAsCanceled();

                        await unitOfWork.CommitAsync(cancellationToken);

                        continue;
                    }

                    if (toAccount == null)
                    {
                        shebaRequest.SetAsCanceled();
                        fromAccount.CancelLock("Destination account does not exist", shebaRequest.Id);

                        await unitOfWork.CommitAsync(cancellationToken);

                        continue;
                    }

                    fromAccount!.WithdrawLockedAmount(shebaRequest.Note, shebaRequest.Id);
                    toAccount.Deposit(shebaRequest.Note, shebaRequest.Price);

                    shebaRequest.SetAsCompleted();
                    command.Update(shebaRequest);

                    accountCommand.UpdateAccountComplete(fromAccount);
                    accountCommand.UpdateAccountComplete(toAccount);

                    await unitOfWork.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);

                    logger.LogError("CancelShebaBatchCommand error: {@e}", e);

                    shebaRequest.SetAsReadyToRetry();
                    command.Update(shebaRequest);

                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    logger.LogDebug("ShebaRequest set for Retry : {@shebaRequest}", shebaRequest);
                }
            }
        }

        return true;
    }
}
