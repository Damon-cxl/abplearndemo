using Acme.Books.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Acme.Books.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(BooksEntityFrameworkCoreDbMigrationsModule),
        typeof(BooksApplicationContractsModule)
        )]
    public class BooksDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
