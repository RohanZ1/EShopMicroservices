using Ordering.Application.Orders.Commands.CreateOrder;
namespace Ordering.API.Endpoints
{
    /*
    1) Accepts a CreateOrderRequest object
    2) Maps the request to CreateOrderCommand
    3) Use MediatR to send the command to the corresponding handler
    4) Returns a response with the created order's ID
     */
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                CreateOrderCommand req= request.Adapt<CreateOrderCommand>();

               CreateOrderResult resp= await sender.Send(req);

                CreateOrderResponse response= resp.Adapt<CreateOrderResponse>();
                return Results.Created($"/orders/{response.Id}", response);
            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Order")
            .WithDescription("Create Order");
           
        }
    }
}
