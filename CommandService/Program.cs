using CommandService.Core;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var logger = new LoggerConfiguration()
        .Enrich.WithThreadId()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithThreadName()
        .Enrich.WithClientAgent()
        .Enrich.WithClientIp()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithExceptionDetails(
            new DestructuringOptionsBuilder()
            .WithDefaultDestructurers()
            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
            .WithDestructurers(new[] { new SqlExceptionDestructurer() }))
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateBootstrapLogger();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog(logger); 
    
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseGlobalErrorHandler();

    app.MapPost("api/cmd/platforms", () =>
    {
        return "Inbound test from the platform  Controller";
    })
    .WithName("TestInboundPlatform");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}