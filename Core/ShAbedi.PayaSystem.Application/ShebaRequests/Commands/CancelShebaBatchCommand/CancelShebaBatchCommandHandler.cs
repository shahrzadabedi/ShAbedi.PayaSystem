using System.Data;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShAbedi.PayaSystem.Application.Common.Contracts;

namespace ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CancelShebaBatchCommand;

public class CancelShebaBatchCommandHandler(
    IShebaQueryRepository query,
    IShebaCommandRepository command,
    IAccountQueryRepository accountQuery,
    IAccountCommandRepository accountCommand,
    IUnitOfWork unitOfWork,
    ILogger<CancelShebaBatchCommandHandler> logger) : IRequestHandler<CancelShebaBatchCommand, bool>
{
    public async Task<bool> Handle(CancelShebaBatchCommand request, CancellationToken cancellationToken)
    {
        var count = await query.CountReadyToCancel(cancellationToken);

        var batchSize = 10;
        int iterationsCount = (int)Math.Ceiling(((float)(count / batchSize)));

        for (int i = 0; i <= iterationsCount; i++)
        {
            var shebaRequests = await query.FindReadyToCancel(batchSize, i + 1, cancellationToken);

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
                    
                    shebaRequest.SetAsCanceled();
                    command.Update(shebaRequest);

                    fromAccount.CancelLock("Canceled By Operator", shebaRequest.Id);
                    accountCommand.UpdateAccountComplete(fromAccount);

                    await unitOfWork.CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await unitOfWork.RollbackAsync(cancellationToken);

                    logger.LogError("CancelShebaBatchCommand error: {@e}", e);
                }
            }
        }

        return true;
    }
}
