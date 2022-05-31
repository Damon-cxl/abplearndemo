using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Acme.Books.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(BooksHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class BooksConsoleApiClientModule : AbpModule
    {
        
    }
}
