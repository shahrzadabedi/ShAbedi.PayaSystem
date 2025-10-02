using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ShAbedi.PayaSystem.Api;
[ApiController]

public class BaseController : ControllerBase
{

    private IMediator _mediatorInstance;
    private IMapper _mapperInstance;
    protected IMediator Mediator => _mediatorInstance ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    protected IMapper Mapper => _mapperInstance ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
}
