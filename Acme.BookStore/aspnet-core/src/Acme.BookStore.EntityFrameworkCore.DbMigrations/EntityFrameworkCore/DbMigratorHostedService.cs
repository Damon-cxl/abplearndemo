using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Acme.BookStore.Data;

namespace Acme.BookStore.EntityFrameworkCore
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbMigratorHostedService"/> class.
        /// </summary>
        /// <param name="serviceProvider">serviceProvider.</param>
        public DbMigratorHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceScope = serviceProvider.CreateScope();
            await serviceScope.ServiceProvider.GetRequiredService<BookStoreDbMigrationService>().MigrateAsync();

            serviceScope.Dispose();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
