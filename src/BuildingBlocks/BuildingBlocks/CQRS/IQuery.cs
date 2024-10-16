using MediatR;


namespace BuildingBlocks.CQRS
{//So, according to CQRS pattern you have seperated Query and Commands with IQuery and ICommand interfaces
    public interface IQuery<TResponse> : IRequest<TResponse>//IQuery only have select commands i.e only fetching data. So, it will always have a return type
    where TResponse : notnull//for read operations
    {
    }
}
//CQRS will help in validations which are different for Command and different for Query