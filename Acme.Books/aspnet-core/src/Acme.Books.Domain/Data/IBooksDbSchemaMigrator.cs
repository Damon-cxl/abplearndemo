using System.Threading.Tasks;

namespace Acme.Books.Data
{
    public interface IBooksDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
