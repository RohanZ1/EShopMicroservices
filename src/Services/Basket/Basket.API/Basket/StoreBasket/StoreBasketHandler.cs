﻿using Basket.API.Data;

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

    public class StoreBasketCommandHandler(IBasketRepository repo) :
        ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart= command.Cart;
            await repo.StoreBasket(cart, cancellationToken);

            //store basket in database (use marten upsert- if exists update,if not insert
            //update cache in redis distributed cache
            return new StoreBasketResult(cart.UserName);
        }
    }
}