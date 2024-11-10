

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);//Primary key
            builder.Property(c => c.Id).HasConversion(
                customerId => customerId.Value,//while writing to database this line from HasConversion converts Non-Primitive type to primitive which can directly be stored in Database like id,guid
                dbId => CustomerId.Of(dbId));//while reading data from table, int/Guid is converted back to CustomerId non-primitive to be further used in application
            //HasConversion basically bridges gap between datatype of column in DB with datatype of Strongly typed entities

            builder.Property(c=>c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255);
            builder.HasIndex(c=>c.Email).IsUnique();
        }
    }
}
