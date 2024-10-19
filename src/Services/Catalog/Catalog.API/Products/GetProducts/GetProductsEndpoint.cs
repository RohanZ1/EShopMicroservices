using Catalog.API.Products.CreateProduct;
using Newtonsoft.Json;

namespace Catalog.API.Products.GetProducts
{
    // public record  GetProductsRequest();
    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductsResponse(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                GetProductsQuery? query = request.Adapt<GetProductsQuery>();
                GetProductResult result = await sender.Send(query);//you pass a command/query objects to sender and it will return corresponding return types defined in their implementations
                GetProductsResponse response = result.Adapt<GetProductsResponse>();//for Mapster to work without configuration make sure to keep variable names of GetProductsResponse(IEnumerable<Product> products) and GetProductResult(IEnumerable<Product> pro)
                string s = JsonConvert.SerializeObject(response, Formatting.Indented);
                return Results.Ok(response);
            })
                 .WithName("GetProducts")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products")
                .WithDescription("Get Products");
        }
    }
}
