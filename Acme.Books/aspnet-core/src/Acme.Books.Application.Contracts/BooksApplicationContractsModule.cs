using Acme.Books.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.VirtualFileSystem;

namespace Acme.Books
{
    [DependsOn(
        typeof(BooksDomainSharedModule),
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpTenantManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class BooksApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BooksDtoExtensions.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Localization used with VirtualFileSystem , VirtualFileSystem for json file resources
            Configure<AbpVirtualFileSystemOptions>(options => options.FileSets.AddEmbedded<BooksApplicationContractsModule>("Acme.Books"));

            Configure<AbpLocalizationOptions>(options =>
            {
                _ = options.Resources.Get<BooksResource>()
                    .AddVirtualJson("/Localization/ApplicationContracts");
            });
        }
    }
}
