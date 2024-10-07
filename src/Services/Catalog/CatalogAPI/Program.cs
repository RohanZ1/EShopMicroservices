using Carter;
using CatalogAPI.Products.CreateProduct;
using Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);//adding all required services into mediatr to let mediatar handle it
});
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database"));

}).UseLightweightSessions();//configure postgre sql fro marten library
//add services via dependency injection
var app = builder.Build();
//configure httprequest pipeline

app.MapCarter();
app.Run();
