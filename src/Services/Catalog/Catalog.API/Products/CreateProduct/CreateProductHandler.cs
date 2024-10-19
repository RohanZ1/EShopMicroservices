

using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
      : ICommand<CreateProductResult>;//similar to inherited a class with properties inside paranthesis which is inheriting from ICommand<TResponse>
    public record CreateProductResult(Guid Id);//similar to a class with one propery Guid Id
                                              
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> //Fluent Validation class for CreateProductCommand slice . Contains all model based rules . Alternative to Attributes
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session) :
     ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand commandRequest, CancellationToken cancellationToken)
        {
            //model validation cross cutting concern was written in every commandhandler/queryhandler class which lead to error prone code.So, we add it in Building blocks behaviours folder
            //var validRes = validator.Validate(commandRequest);
            //var listErr = validRes.Errors.Select(err => err.ErrorMessage).ToList();
            //if (listErr.Any())
            //{
            //    throw new ValidationException(listErr.FirstOrDefault());
            //}
            var product = new Product
            {
                Id = commandRequest.Id,
                Name = commandRequest.Name,
                Category = commandRequest.Category,
                Description = commandRequest.Description,
                ImageFile = commandRequest.ImageFile,
                Price = commandRequest.Price

            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);//generate from document db of marten library
        }
    }
}
