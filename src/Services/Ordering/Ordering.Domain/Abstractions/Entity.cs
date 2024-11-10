
namespace Ordering.Domain.Abstractions
{
    public abstract class Entity<T> : IEntity<T>//base entity class abstract
    {
        public T Id { get; set; }//an Entity because it has unique identifier i.e ID
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
       
    }
}
