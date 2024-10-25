

namespace Basket.API.Basket.DeleteBasket
{
   // public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBaskerEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async(string UserName, ISender sender)=>{

                var result = await sender.Send(new DeleteBasketCommand(UserName));
                var response=result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            });
           
        }
    }
}
