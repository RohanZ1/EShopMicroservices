
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)//passing options which are injected via DI to base Dbcontext constructor
        {
            
        }
        public DbSet<Customer> Customers => Set<Customer>();//DbSet for each Entities which have tables for each of them as well
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Customer>()//This kind of configureation we need to apply for all the entities which makes this OnModelCreating method very lengthy. So, we create seperate classes for each of them 
            //            .Property(c => c.Name)
            //            .IsRequired()
            //            .HasMaxLength(100);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//this will load configurations from different classes in assembly
            //this will scan assembly for implementations of IEntityTypeConfiguration<T> and add configurations from all implementations classes in assembly into OnModelCreating without makeing code lengthy
            base.OnModelCreating(builder);
        }
    }
}
