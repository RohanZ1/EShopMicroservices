using Catalog.API.Products.CreateProduct;
using Mapster;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                GetProductByIdResult? result = await sender.Send(new GetProductByIdQuery(id));
                GetProductByIdResponse response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok<GetProductByIdResponse>(response);
            })
                .WithName("GetProductById")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product by Id")
                .WithDescription("Get Product by Id");

        }
    }
}
