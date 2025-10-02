using Hangfire;
using Hangfire.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ShAbedi.PayaSystem.Application.Common.Contracts;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.CompleteShebasCommand;
using ShAbedi.PayaSystem.Application.ShebaRequests.Commands.ShabaCommand;
using ShAbedi.PayaSystem.Infrastructure.Persistence;
using ShAbedi.PayaSystem.Infrastructure.Repositories;
using ShAbedi.PayaSystem.Jobs.Jobs;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
Serilog.Debugging.SelfLog.Enable(Console.Out);

// Add services to the container.
var hangfireConnection = builder.Configuration.GetConnectionString("Hangfire");

builder.Services.AddHangfire(x => x.UseSqlServerStorage(hangfireConnection));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountQueryRepository, AccountQueryRepository>();
builder.Services.AddScoped<IAccountCommandRepository, AccountCommandRepository>();
builder.Services.AddScoped<IShebaCommandRepository, ShebaCommandRepository>();
builder.Services.AddScoped<IShebaQueryRepository, ShebaQueryRepository>();

var cs = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(cs); }, ServiceLifetime.Scoped);

builder.Services.AddAutoMapper(cfg =>
{
}, typeof(ShebaCommand).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CompleteShebaBatchCommand>();
});

var app = builder.Build();

app.UseHangfireDashboard();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var recurring = app.Services.GetRequiredService<IRecurringJobManager>();

//recurring.AddOrUpdate(
//    recurringJobId: "CompleteShebaRequestsJob",
//    job: Job.FromExpression<CompleteShebaRequestsJob>(job =>
//    job.ExecuteAsync(CancellationToken.None)),
//    cronExpression: "*/5 * * * *",
//    options: new RecurringJobOptions());

//recurring.AddOrUpdate(
//    recurringJobId: "CancelShebaRequestsJob",
//    job: Job.FromExpression<CancelShebaRequestsJob>(job =>
//    job.ExecuteAsync(CancellationToken.None)),
//    cronExpression: "*/5 * * * *",
//    options: new RecurringJobOptions());

//recurring.AddOrUpdate(
//    recurringJobId: "RetryCompleteShebaRequestsJob",
//    job: Job.FromExpression<RetryCompleteShebaRequestsJob>(job =>
//    job.ExecuteAsync(CancellationToken.None)),
//    cronExpression: "*/5 * * * *",
//    options: new RecurringJobOptions());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
