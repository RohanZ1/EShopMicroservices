using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;


namespace BuildingBlocks.Behaviors
{
    public class ValidationBehaviour<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>//use this validator only if a request comes from command 
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //Task.WhenAll accepts a collection of async Tasks
            //flow will stop until every Task from collection is completed
            var faiures = validationResult.Where(v => v.Errors.Any()).SelectMany(e => e.Errors).ToList();
            if (faiures.Any())
            {
                throw new ValidationException(faiures);
            }
            return await next();//calls next pipeline behaviour or handle method of command
        }
    }
}
