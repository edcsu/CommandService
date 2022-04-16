using CommandService.AsyncDataServices;
using CommandService.Business.Config;
using CommandService.Business.EventProcessor;
using CommandService.Business.Repositories.Implementations;
using CommandService.Business.Repositories.Interfaces;
using CommandService.Business.Services;
using CommandService.Business.ViewModels;
using CommandService.Core;
using CommandService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;

Log.Logger = new LoggerConfiguration()
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
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    var seqConfig = builder.Configuration.GetSeqSettings();
    
    builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Seq(seqConfig.Url)
       .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseInMemoryDatabase("Commandmemo"));

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
    builder.Services.AddScoped<ICommandRepository, CommandRepository>();

    builder.Services.AddScoped<ICommandService, CommandService.Business.Services.CommandService>();
    
    builder.Services.AddSingleton<IEventProcessor, EventProcessor>();

    builder.Services.AddHostedService<MessageBusSubscriber>();

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
        return "Inbound test from the platform Controller";
    })
    .WithTags( new string[] { "Platforms" })
    .Produces(statusCode: StatusCodes.Status200OK, responseType: typeof(PlatformDetailsDto))
    .Produces(statusCode: StatusCodes.Status404NotFound)
    .Produces(statusCode: StatusCodes.Status400BadRequest)
    .WithMetadata(new RouteNameMetadata("CreatePlatform"))
    .WithName("CreatePlatform");

    app.MapGet("api/cmd/platforms", ([FromServices] ICommandService _commandService) =>
    {
        return Results.Ok(_commandService.GetAllPlatformsAsync());
    })
    .WithMetadata(new RouteNameMetadata("GetAllPlatforms"))
    .WithName("GetAllPlatforms")
    .WithTags( new string[] { "Platforms" })
    .Produces(statusCode: StatusCodes.Status200OK, responseType: typeof(IEnumerable<PlatformDetailsDto>));


    app.MapGet("api/cmd/platforms/{platformId:guid}/commands", 
        ([FromServices] ICommandService _commandService, Guid platformId) =>
    {
        var commands = _commandService.GetAllCommandsForPlatform(platformId);
        return commands is null ? Results.NotFound() : Results.Ok(commands);
    })
    .WithMetadata(new RouteNameMetadata("GetAllCommandsForPlatform"))
    .WithName("GetAllCommandsForPlatform")
    .WithTags( new string[] { "Commands" })
    .Produces(statusCode: StatusCodes.Status200OK, responseType: typeof(IEnumerable<CommandDetailsDto>))
    .Produces(statusCode: StatusCodes.Status404NotFound);

    app.MapGet("api/cmd/platforms/{platformId:guid}/commands/{commandId:guid}", 
        ([FromServices] ICommandService _commandService, Guid platformId, Guid commandId) =>
    {
        var command = _commandService.GetCommand(platformId, commandId);
        return command is null ? Results.NotFound() : Results.Ok(command);
    })
    .WithName("GetCommandForPlatform")
    .WithTags( new string[] { "Commands" })
    .WithMetadata(new RouteNameMetadata("GetCommandForPlatform"))
    .WithMetadata(new EndpointNameMetadata("get_command_for_platform"))
    .Produces(statusCode: StatusCodes.Status200OK, responseType: typeof(CommandDetailsDto))
    .Produces(statusCode: StatusCodes.Status404NotFound);

    app.MapPost("api/cmd/platforms/{platformId:guid}/commands", 
        async ([FromServices] ICommandService _commandService, Guid platformId,
        [FromBody] CommandCreateDto commandCreateDto) =>
    {
        var command = await _commandService.CreateCommandAsync(platformId, commandCreateDto);
        return command is null ? 
            Results.NotFound() : Results.CreatedAtRoute("GetCommandForPlatform", new { platformId, commandId = command.Id}, command);
    })
    .WithMetadata(new RouteNameMetadata("CreateCommandForPlatform"))
    .WithName("CreateCommandForPlatform")
    .WithTags( new string[] { "Commands" })
    .Produces(statusCode: StatusCodes.Status201Created, responseType: typeof(CommandDetailsDto))
    .Produces(statusCode: StatusCodes.Status404NotFound);

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