using Acme.Books.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Acme.Books;
using System.Collections.Generic;
using Volo.Abp.Uow;

namespace Acme.Books.Controllers
{
    /* Inherit your controllers from this class.
     */
    [Route("api/v1/[controller]")]
    public class BooksController : AbpController
    {
        private IBooksAppService service;
        public BooksController(IBooksAppService service)
        {
            // LocalizationResource = typeof(BooksResource);
            this.service = service;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<object> GetTestAsync([FromBody]CreateBookQuery input)
        {
            return await service.GetTestAsync(input);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<object> GetTest2Async([FromBody]List<CreateBookQuery> input)
        {
            return await service.GetTestAsync(input[0]);
        }

        [HttpPost]
        [Route("[action]")]
        [UnitOfWork(IsDisabled = true)]
        public async Task<object> NewBooks([FromBody] List<CreateBookQuery> input)
        {
            return await service.CreateBooksAsync(input[0]);
        }
    }
}