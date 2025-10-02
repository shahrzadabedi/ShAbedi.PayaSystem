using System.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShAbedi.PayaSystem.Application.Common.Contracts;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.RetryCompleteShebaBatchCommand;

public class RetryCompleteShebaBatchCommandHandler(
    IShebaQueryRepository query,
        IShebaCommandRepository command,
        IAccountQueryRepository accountQuery,
    IAccountCommandRepository accountCommand,
        IUnitOfWork unitOfWork,
        ILogger<RetryCompleteShebaBatchCommandHandler> logger) : IRequestHandler<RetryCompleteShebaBatchCommand,bool>
{
    public async Task<bool> Handle(RetryCompleteShebaBatchCommand request, CancellationToken cancellationToken)
    {
        var retryLimit = 3;
        var count = await query.CountReadyToRetry(retryLimit, cancellationToken);

        var batchSize = 10;
        int iterationsCount = (int)Math.Ceiling(((float)(count / batchSize)));

        for (int i = 0; i <= iterationsCount; i++)
        {
            var shebaRequests = await query.FindReadyToRetry(batchSize, i + 1, retryLimit, cancellationToken);

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
                    var fromAccount = await accountQuery.FindByShebaNumber(shebaRequest.FromShebaNumber, cancellationToken);
                    var toAccount = await accountQuery.FindByShebaNumber(shebaRequest.ToShebaNumber, cancellationToken);

                    if (fromAccount == null)
                    {
                        shebaRequest.SetAsCanceled();
                        command.Update(shebaRequest);

                        await unitOfWork.CommitAsync(cancellationToken);

                        continue;
                    };
                    if (toAccount == null)
                    {
                        shebaRequest.SetAsCanceled();
                        command.Update(shebaRequest);

                        fromAccount.CancelLock("Destination account does not exist", shebaRequest.Id);
                        accountCommand.UpdateAccountComplete(fromAccount);

                        await unitOfWork.CommitAsync(cancellationToken);

                        continue;
                    }

                    if (shebaRequest.RetryCount == retryLimit)
                    {
                        shebaRequest.SetAsFailed();
                        command.Update(shebaRequest);

                        fromAccount.FailLock("Maximum retry limit reached", shebaRequest.Id);
                        accountCommand.UpdateAccountComplete(fromAccount);

                        await unitOfWork.CommitAsync(cancellationToken);

                        continue;
                    }

                    fromAccount!.WithdrawLockedAmount(shebaRequest.Note, shebaRequest.Id);
                    toAccount.Deposit(shebaRequest.Note, shebaRequest.Price);

                    shebaRequest.SetAsCompleted();
                    shebaRequest.IncreaseRetryCount();
                    command.Update(shebaRequest);

                    accountCommand.UpdateAccountComplete(fromAccount);
                    accountCommand.UpdateAccountComplete(toAccount);

                    await unitOfWork.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);

                    logger.LogError("CancelShebaBatchCommand error: {@ex}", ex);

                    shebaRequest.SetAsReadyToRetry();
                    shebaRequest.IncreaseRetryCount();
                    command.Update(shebaRequest);

                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    logger.LogDebug("ShebaRequest set for Retry : {@shebaRequest}", shebaRequest);
                }
            }
        }

        return true;
    }
}
