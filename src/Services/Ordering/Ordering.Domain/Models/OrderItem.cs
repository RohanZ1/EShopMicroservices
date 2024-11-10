

namespace Ordering.Domain.Models
{
    public class OrderItem :Entity<OrderItemId>//Guid is ID for OrderItem Entity
    {
        internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
        {//access modifierr of OrderItem is internal as orderitem should only be created from Order Aggregate
            Id=OrderItemId.Of(Guid.NewGuid());
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
        public OrderId OrderId { get; private set; } = default!;
        public ProductId ProductId { get; private set; } = default!;
        public int Quantity { get; private set; }=default!;
        public decimal Price { get; private set;}=default!;
    }
}
