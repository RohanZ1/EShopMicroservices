using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

    public record GetOrdersByNameHandler(IApplicationDbContext dbContext) :
        IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
        /*
         Include() is an eager loading operation. When you call Include, you are instructing EF Core to load related entities along with the Order entities
        This specifies that the OrderItems navigation property (likely a collection of OrderItem entities) should also be loaded alongside the Order entity. 
        Without this, if you access OrderItems later, it would trigger another query to load them (lazy loading). 
        The Include method prevents that by loading the related data upfront in the same query


        .AsNoTracking() When you query the database, by default, EF Core tracks the entities it retrieves, which can be useful for updating them later.
        However, if you are only reading the data and don’t need to update it, using AsNoTracking() can improve performance because it reduces memory overhead and avoids unnecessary tracking of the entities in the context.
         */
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)//for eager loading of OrderItems navigation property 
            .AsNoTracking()// This is particularly helpful in read-only scenarios, where you don't need to modify the entities or track changes to them.
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(cancellationToken);

        //var orders = await dbContext.Orders
        //    .Include(o => o.OrderItems)
        //    .AsNoTracking()
        //    .Where(o => o.OrderName.Value.Contains(query.Name))
        //    .OrderBy(o => o.OrderName.Value)
        //    .ToListAsync(cancellationToken);
        return new GetOrdersByNameResult(orders.ToOrderDtoList());
            
        }
        //private List<OrderDto> ProjectToOrderDto(List<Order> orders)
        //{
        //    List<OrderDto> result = new();
        //    foreach(var order in orders)
        //    {
        //        var orderDto = new OrderDto(
        //            Id: order.Id.Value,
        //            CustomerId: order.CustomerId.Value,
        //            OrderName: order.OrderName.Value,
        //            ShippingAddress: new AddressDto(
        //                order.ShippingAddress.FirstName,
        //                order.ShippingAddress.LastName,
        //                order.ShippingAddress.EmailAddress,
        //                order.ShippingAddress.AddressLine,
        //                order.ShippingAddress.Country,
        //                order.ShippingAddress.State,
        //                  order.ShippingAddress.ZipCode),
        //            BillingAddress: new AddressDto(
        //                order.BillingAddress.FirstName,
        //                order.BillingAddress.LastName,
        //                order.BillingAddress.EmailAddress,
        //                order.BillingAddress.AddressLine,
        //                order.BillingAddress.Country,
        //                order.BillingAddress.State,
        //                  order.BillingAddress.ZipCode
        //                ),
        //            Payment: new PaymentDto(
        //                order.Payment.CardName,
        //                order.Payment.CardNumber,
        //                order.Payment.Expiration,
        //                order.Payment.CVV,
        //                order.Payment.PaymentMethod),
        //            Status: order.Status,
        //            OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList());
        //        result.Add(orderDto);
        //    }
        //    return result;
        //}
    }


