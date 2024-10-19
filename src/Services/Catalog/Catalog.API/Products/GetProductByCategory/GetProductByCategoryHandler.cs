namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var prodList = await session.Query<Product>()
                                 .Where(prod => prod.Category
                                 .Contains(query.category))
                                 .ToListAsync(cancellationToken);
            return new GetProductByCategoryResult(prodList);
        }
    }
}
