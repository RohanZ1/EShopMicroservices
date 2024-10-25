

using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string Username) 
        : IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart Cart);
    public class GetBasketQueryHandler(IBasketRepository repo) :
        IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            //TODO: get basket from database
             var basket=await repo.GetBasket(query.Username);
            return new GetBasketResult(basket);
        }
    }
}
