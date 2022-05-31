using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Acme.Books.EntityFrameworkCore
{
    [DependsOn(
        typeof(BooksEntityFrameworkCoreModule)
        )]
    public class BooksEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BooksMigrationsDbContext>();
        }
    }
}
