using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Acme.Books.Data;
using Volo.Abp.DependencyInjection;

namespace Acme.Books.EntityFrameworkCore
{
    public class EntityFrameworkCoreBooksDbSchemaMigrator
        : IBooksDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreBooksDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the BooksMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<BooksMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}