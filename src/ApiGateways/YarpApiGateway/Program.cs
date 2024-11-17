using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//Add services to DI containmer

builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);//in 10 seconds , we can send maximum 5 request
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

//configure request pipeline
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
