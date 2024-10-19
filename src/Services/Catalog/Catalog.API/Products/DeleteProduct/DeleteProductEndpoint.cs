namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid id);//will pass through route so not required
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                DeleteProductResult result = await sender.Send(new DeleteProductCommand(id));
                DeleteProductResponse response = result.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            });
        }
    }
}
