﻿using BuildingBlocks.CQRS;
using CatalogAPI.Models;
using Marten;
using MediatR;

namespace CatalogAPI.Products.CreateProduct;

public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;//similar to inherited a class with properties inside paranthesis which is inheriting from ICommand<TResponse>
public record CreateProductResult(Guid Id);//similar to a class with one propery Guid Id

internal class CreateProductCommandHandler(IDocumentSession session) :
    ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand commandRequest, CancellationToken cancellationToken)
    {
        var product = new Product
        {
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
