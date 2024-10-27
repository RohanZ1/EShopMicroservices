using Basket.API.Data;
using Discount.Grpc;
using NetTopologySuite.Index.HPRtree;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart):ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator:
        AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");

        }
    }

    public class StoreBasketCommandHandler(IBasketRepository repo,DiscountProtoService.DiscountProtoServiceClient discpuntproto) :
        ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)//is called when you add or remove items into shopping cart
        {//This method will consume grpc service so this method is defined as client for grpc which only invokes grpc requests and discount.grpc is a server which only server request
         //communicate with discount.grpc and calculate latest prices of products

             await DeductDiscount(command.Cart, cancellationToken);
             await repo.StoreBasket(command.Cart, cancellationToken);

            //store basket in database (use marten upsert- if exists update,if not insert
            //update cache in redis distributed cache
            return new StoreBasketResult(command.Cart.UserName!);
        }
        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)//this method will deduct discount amount from al items in shoppingcart and return to handle method
        {
            // Communicate with Discount.Grpc and calculate lastest prices of products into sc
            foreach (var item in cart.Items)
            {
                var coupon = await discpuntproto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                item.Price -= coupon.Amount;
            }
        }
    }
   
}
