

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private const int DefaultLength = 5;//for validation of OrderName as its Rich domain so domain specific validations are there in ValueObjects itself
        public string Value { get; }
        private OrderName(string value) => Value = value;
        public static OrderName Of(string value)//of method is used to instantiate ValueObjects inside Of method by uysing a private constructor . This way we can give validations to fields before instantiating
        {//encapsulated all the relevant properties and buisness rules within the Of method for each ValueObject.
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value);
         //   ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);
           
            return new OrderName(value);
        }
    }
}
