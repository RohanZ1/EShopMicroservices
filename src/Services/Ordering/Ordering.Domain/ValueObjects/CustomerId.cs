using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record CustomerId// we have created strongly type ID value so as to not pass Customerid in place of order id and vice versa. Strongly typed nature provides compile time checking of correct id passing into a method
    {
        public Guid Value { get; }
        private CustomerId(Guid value)=>Value= value;
        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("CustomerId cannot be empty");
            }
            return new CustomerId(value);
        }
    }
}
