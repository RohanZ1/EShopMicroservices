var builder = WebApplication.CreateBuilder(args);

//Add services to DI containmer


var app = builder.Build();

//configure request pipeline


app.Run();
