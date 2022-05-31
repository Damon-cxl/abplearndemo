using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.EntityFrameworkCore
{
    public class DataContextSeed : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> parameterRepository;
        private readonly IHostEnvironment env;
        private readonly ILogger<DataContextSeed> logger;

        public DataContextSeed(
            IRepository<Book, Guid> parameterRepository,
            IHostEnvironment env,
            ILogger<DataContextSeed> logger)
        {
            this.parameterRepository = parameterRepository;
            this.env = env;
            this.logger = logger;
        }

        /// <summary>
        ///    Seed asynchronous.
        /// </summary>
        /// <param name="env">      The environment. </param>
        /// <param name="logger">   The logger. </param>
        /// <returns>   An asynchronous result. </returns>

        public virtual async Task SeedAsync(DataSeedContext context)
        {
            if (await parameterRepository.GetCountAsync() == 0)
            {
                await parameterRepository.GetDbContext().AddRangeAsync(InitParameters());
                await parameterRepository.GetDbContext().SaveChangesAsync();
            }

            var policy = CreatePolicy(logger, nameof(DataContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var contentRootPath = env.ContentRootPath;


            });
        }

        private IEnumerable<Book> InitParameters() => new List<Book>
        {
            new Book
            {
                Name = "book1",
                Type = BookType.Adventure,
                PublishDate = DateTime.Now,
                Price = 120
            },
        };


        private AsyncPolicy CreatePolicy(ILogger<DataContextSeed> logger, string prefix, int retries = 3) => Policy.Handle<SqlException>().WaitAndRetryAsync(
                retries,
                _ => TimeSpan.FromSeconds(5),
                (exception, _, retry, __) => logger.LogTrace(
                        $"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry.ToString()} of {retries.ToString()}"));
    }
}
