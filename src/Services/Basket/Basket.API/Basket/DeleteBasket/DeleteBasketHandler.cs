﻿

using Basket.API.Data;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);
   
    public class DeleteCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(x=>x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }       
public class DeleteBasketCommandHandler(IBasketRepository repo)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
            //Delete basket from Database and cache     
            //session.Delete<Product>(command.Id);
            await repo.DeleteBasket(command.UserName, cancellationToken);
            return new DeleteBasketResult(true);
    }
}
}
