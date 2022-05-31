using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Acme.Books.Data
{
    /* This is used if database provider does't define
     * IBooksDbSchemaMigrator implementation.
     */
    public class NullBooksDbSchemaMigrator : IBooksDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}