using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CompleteShebasCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.RetryCompleteShebaBatchCommand;

namespace ShAbedi.PayaSystem.Jobs.Jobs;

public class RetryCompleteShebaRequestsJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CompleteShebaRequestsJob> logger;

    public RetryCompleteShebaRequestsJob(
        IServiceProvider serviceProvider,
        ILogger<RetryCompleteShebaRequestsJob> logger)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var command = new RetryCompleteShebaBatchCommand();

                await mediator.Send(command, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error executing RetryCompleteShebaRequestsJob: {@ex}", ex);
        }
    }
}
