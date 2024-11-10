using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Add services to container
/*
 Infrastructure- EF core dependency
Application - MediatR i.e CQRS design pattern
API- Carter,HealthChecks,...
 */
builder.Services
       .AddApplicationServices()//mediatr dependencies added in Application layer
       .AddInfrastructureServices(builder.Configuration)//mediatr is used in Infrastructure layers for Intersecptors. So, we need to add medatR before adding Infra layer
       .AddApiServices(builder.Configuration);



var app = builder.Build();
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();//auto migration and seeding
}
//Configure http request pipeline
app.Run();
