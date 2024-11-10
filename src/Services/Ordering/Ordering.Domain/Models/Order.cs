

namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>//Order is Aggregate root it contains Entity Order and OrderItem as well as Value Objects like Address,Payment
    {//Order Aggregate is a Rich Domain Model as it contains Buisness logic along with data getter and setters
        private List<OrderItem> _orderItems = new();//Guid is ID for OrderItem Entity
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();//remember List you are exposing to outside of application should always be readonly list

        public CustomerId CustomerId { get; private set; } = default!;//replaced Guid with strongly type CustomerId
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Quantity);
            private set { }
        }
        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {//create a new order for a specific customer
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending,
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));//OrderCreatedEvent inherits from IDomainEvent
            return order;
        }
        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
        {//update order details for a specific customer
            OrderName = orderName;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedEvent(this));
        }
        public void Add(ProductId productId, int quantity, decimal price)
        {//adding new OrderItem to Order aggergate
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
            OrderItem orderItem = new OrderItem(Id, productId, quantity, price);  //Id field in Entity<T> is Id of that specific entity
            _orderItems.Add(orderItem);// Id is OrderId for Order Entity
        }
        public void Remove(ProductId productId)
        {
            var orderitem=_orderItems.FirstOrDefault(x => x.ProductId == productId);
            if(orderitem is not null) _orderItems.Remove(orderitem);
        }
    }
}
