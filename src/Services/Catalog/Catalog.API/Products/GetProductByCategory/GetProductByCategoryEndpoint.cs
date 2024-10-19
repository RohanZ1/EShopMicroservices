using Mapster;

namespace Catalog.API.Products.GetProductByCategory
{
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        // public record GetProductByCategoryRequest(string category);// Not required
        public record GetProductByCategoryResponse(IEnumerable<Product> Products);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                GetProductByCategoryResult? result = await sender.Send(new GetProductByCategoryQuery(category));
                GetProductByCategoryResponse response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            });

        }
    }
}
