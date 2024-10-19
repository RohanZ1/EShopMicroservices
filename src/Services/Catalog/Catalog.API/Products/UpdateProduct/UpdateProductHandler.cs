using Catalog.API.Exceptions;
using FluentValidation;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
         : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>//we only have to add this class in every command vhandler to call common validation logic forall commands
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").Length(2, 150).WithMessage("Must be between 2 and 150 characters");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            Product? result = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (result is null) throw new ProductNotFoundException(command.Id);
            result.Name = command.Name;
            result.Category = command.Category;
            result.Description = command.Description;
            result.ImageFile = command.ImageFile;
            result.Price = command.Price;
            session.Update(result);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}
