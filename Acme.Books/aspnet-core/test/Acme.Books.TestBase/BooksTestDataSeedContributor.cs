using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.Books
{
    public class BooksTestDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> repository;

        public BooksTestDataSeedContributor(
            IRepository<Book, Guid> repository
            )
        {
            this.repository = repository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            /* Seed additional test data... */
            await repository.InsertAsync(new Book { Name = "Test" });
        }
    }
}