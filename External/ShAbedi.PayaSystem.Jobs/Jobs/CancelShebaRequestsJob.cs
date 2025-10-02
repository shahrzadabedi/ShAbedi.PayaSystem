using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CancelShebaBatchCommand;

namespace ShAbedi.PayaSystem.Jobs.Jobs;

public class CancelShebaRequestsJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CompleteShebaRequestsJob> logger;

    public CancelShebaRequestsJob(
        IServiceProvider serviceProvider,
        ILogger<CancelShebaRequestsJob> logger)
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

                var command = new CancelShebaBatchCommand();

                await mediator.Send(command, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error executing CancelShebaRequestsJob: {@ex}", ex);
        }
    }
}
