using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using ShAbedi.PayaSystem.Api;
using ShAbedi.PayaSystem.Application;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CancelShebaCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.FinalizeShebaCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.DTOs;
using ShAbedi.PayaSystem.Application.ShebaRequests.Queries;

namespace ShAbedi.PayaSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class ShebaController(ILogger<ShebaController> logger) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Sheba([FromBody] ShebaCommandRequest request, CancellationToken cancellationToken)
    {
        var shebaRequestId = Guid.NewGuid();
        var command = Mapper.Map<ShebaCommand>(request);
        command.ShebaRequestId = shebaRequestId;

        var enrichers = new List<IDisposable>()
        {
            LogContext.PushProperty("RequestId", shebaRequestId.ToString())
        };
        
        using (new DisposableEnricherScope(enrichers))
        {
            var result = await Mediator.Send(command, cancellationToken);

            return Ok(result);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ShebaQueryRequest request, CancellationToken cancellationToken)
    {
        var query = Mapper.Map<ShebaQuery>(request);

        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{requestId}")]
    public async Task<IActionResult> Put(Guid requestId, [FromBody] FinalizeShebaCommandRequest request, CancellationToken cancellationToken)
    {
        var enrichers = new List<IDisposable>()
        {
                LogContext.PushProperty("RequestId", requestId.ToString())
        };

        using (new DisposableEnricherScope(enrichers))
        {
            ShebaCommandResponse result;
            if (request.Status == "confirm")
            {
                var command = Mapper.Map<ConfirmShebaCommand>(request);
                command.RequestId = requestId;

                result = await Mediator.Send(command, cancellationToken);
            }
            else
            {
                var command = Mapper.Map<CancelShebaCommand>(request);
                command.RequestId = requestId;

                result = await Mediator.Send(command, cancellationToken);
            }

            return Ok(result);
        }
    }
}
