
using Basket.API.Data;
using BuildingBlocks.Exceptions.Handlers;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container 
//Application Services
var assembly =typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    
});
//Data Services
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x=>x.UserName);//this overrides UserName field as Id of Marten document 
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository,BasketRepository>();
//builder.Services.AddScoped<IBasketRepository,CachedBasketRepository>();
#region Without Scrutor library we add decorated service
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    BasketRepository? basketrepository =provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketrepository, provider.GetService<IDistributedCache>()!);
//});
#endregion
#region with scrutor library to  add decorators to DI container
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();// CachedBasketRepository is another layer of abstraction above BasketRepository. We did this without changing basket repository code

#endregion
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler//added to fix SSL certificate exception. Recommended to use only in development environment
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
//.ConfigureAdditionalHttpMessageHandlers(() =>
//{
//    var handler = new HttpClientHandler
//    {
//        ServerCertificateCustomValidationCallback=HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//    };
//    return handler;
//});


builder.Services.AddExceptionHandler<CustomExceptionHandlers>();
builder.Services.AddHealthChecks()
       .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
       .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

app.UseExceptionHandler(options => { });//disable default behavior so that custom behavior can be applicable
app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse//This UIResponseWriter will make our Healthcheck response into json format
    });
//Configure HTTP request pipeline.
app.MapCarter();
app.Run();
