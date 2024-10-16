

using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommand : ICommand<Unit>//This ICommand is implemented for operations that does not require any return value like delete or update
    {
    }
    public interface ICommand<out TResponse> : IRequest<TResponse>//This ICommand is used when you want to return TResponse
    {
    }
}
