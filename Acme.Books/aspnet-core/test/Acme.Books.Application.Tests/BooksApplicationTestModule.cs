using Volo.Abp.Modularity;

namespace Acme.Books
{
    [DependsOn(
        typeof(BooksApplicationModule),
        typeof(BooksDomainTestModule)
        )]
    public class BooksApplicationTestModule : AbpModule
    {

    }
}