using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AL.Infrastructure.Audit
{
    public class CustomSaveChangesInterceptor : ISaveChangesInterceptor
    {
        public void BeforeSaveChanges(DbContext context)
        {
            Console.WriteLine("Before saving changes");

            // Access and modify entities before saving changes
            var modifiedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .Select(e => e.Entity).ToList();

            foreach (var entity in modifiedEntities)
            {
                // Modify the entity before saving changes
                // ...
            }
        }

        public void AfterSaveChanges(DbContext context)
        {
            Console.WriteLine("After saving changes");

            // Access entities after saving changes
            var insertedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity).ToList();

            var updatedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified)
                .Select(e => e.Entity).ToList();

            var deletedEntities = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted)
                .Select(e => e.Entity);

            // Process inserted, updated, and deleted entities
            // ...
        }

        public Task BeforeSaveChangesAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            // Async version of BeforeSaveChanges
            return Task.CompletedTask;
        }

        public Task AfterSaveChangesAsync(DbContext context, CancellationToken cancellationToken = default)
        {
            // Async version of AfterSaveChanges
            return Task.CompletedTask;
        }

        public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            throw new NotImplementedException();
        }

        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            throw new NotImplementedException();
        }

        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
            throw new NotImplementedException();
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
