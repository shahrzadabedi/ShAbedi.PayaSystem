using Microsoft.EntityFrameworkCore;
using Serilog;
using ShAbedi.PayaSystem.Api.Extensions;
using ShAbedi.PayaSystem.Api.Middlewares;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;
using ShAbedi.PayaSystem.Domain.Contracts;
using ShAbedi.PayaSystem.Infrastructure.Persistence;
using ShAbedi.PayaSystem.Infrastructure.Persistence.Initialization;
using ShAbedi.PayaSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Log
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
Serilog.Debugging.SelfLog.Enable(Console.Out);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
builder.Services.AddScoped<IAccountCommandRepository, AccountCommandRepository>();
builder.Services.AddScoped<IShebaCommandRepository, ShebaCommandRepository>();
builder.Services.AddScoped<IShebaQueryRepository, ShebaQueryRepository>();
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(cs); }, ServiceLifetime.Scoped);

builder.Services.AddAutoMapper(cfg =>
{
}, typeof(ShebaCommand).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ShebaCommand>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

await app.Services.InitializeDatabasesAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
