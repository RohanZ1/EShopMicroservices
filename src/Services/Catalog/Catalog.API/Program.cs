

using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handlers;
using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);//adding all required services into mediatr to let mediatar handle it
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddLogging();
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);

}).UseLightweightSessions();//configure postgre sql fro marten library
if (builder.Environment.IsDevelopment()) builder.Services.InitializeMartenWith<CatalogInitialData>();
builder.Services.AddExceptionHandler<CustomExceptionHandlers>();
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);
var app = builder.Build();
app.MapCarter();
app.UseExceptionHandler(options => { });//empty options object signifies that we are relying on customexception handling behavior and not the default one
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
