

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);

    public record StoreBasketResponse(string UserName);
    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest command, ISender sender) =>
            {
                var cmd = command.Adapt<StoreBasketCommand>();
                var res=await sender.Send(cmd);
                var response=res.Adapt<StoreBasketResponse>();
                return Results.Created($"/basket/{response.UserName}", response);
            });
            
        }
    }
}
