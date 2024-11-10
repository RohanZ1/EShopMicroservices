 using Microsoft.EntityFrameworkCore.ChangeTracking;
 using Microsoft.EntityFrameworkCore.Diagnostics;
namespace Ordering.Infrastructure.Data.Interceptors
{//EF Core SavechangesInterceptor is used for dispatching domain events
    public class AuditableEntityInterceptor : SaveChangesInterceptor// Interceptor is use for automatic auditing whenever entities are saving/updating/creating
    {//flow will come here when you call SaveChanges/SaveChangesAsync method
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntites(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntites(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        public void UpdateEntites(DbContext? context)
        {
            if (context == null) return;
            foreach(var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "mehmet";//createdby for all entities inheriting from IEntity
                    entry.Entity.CreatedAt= DateTime.UtcNow;//createdby for all entities inheriting from IEntity
                }
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = "mehmet";//LastModifiedBy for all entities inheriting from IEntity
                    entry.Entity.LastModified = DateTime.UtcNow;//LastModified for all entities inheriting from IEntity
                }
            }
        }
    }
    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>//this method identifies wheteher entity is changed or not
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }

}
