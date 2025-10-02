using MediatR;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CompleteShebasCommand;

namespace ShAbedi.PayaSystem.Jobs.Jobs
{
    public class CompleteShebaRequestsJob
    {
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<CompleteShebaRequestsJob> logger;

        public CompleteShebaRequestsJob(
			IServiceProvider serviceProvider,
			ILogger<CompleteShebaRequestsJob> logger) 
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

					var command = new CompleteShebaBatchCommand();

					await mediator.Send(command, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				logger.LogError("Error executing CompletePayaRequestsJob: {@ex}", ex);
			}
        }
    }
}
