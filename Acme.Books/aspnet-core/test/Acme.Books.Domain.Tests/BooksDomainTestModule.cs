using Acme.Books.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Acme.Books
{
    [DependsOn(
        typeof(BooksEntityFrameworkCoreTestModule)
        )]
    public class BooksDomainTestModule : AbpModule
    {

    }
}