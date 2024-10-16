var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);//adding all required services into mediatr to let mediatar handle it
    //config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    //config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddLogging();
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);

}).UseLightweightSessions();//configure postgre sql fro marten library
var app = builder.Build();
app.MapCarter();



app.Run();
