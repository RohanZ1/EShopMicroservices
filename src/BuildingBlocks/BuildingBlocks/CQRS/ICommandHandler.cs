using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommandHandler<in TCommand>//if you don't get response from command i.e return void so unit is used
        : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>

    {

    }
    public interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull//when you get a TResponse from command 
    {
    }
}
