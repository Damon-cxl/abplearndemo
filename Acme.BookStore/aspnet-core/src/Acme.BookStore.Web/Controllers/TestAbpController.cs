using Acme.BookStore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.BookStore.Controllers
{
    /* Inherit your controllers from this class.
     */
    [Route("api/v1/[controller]")]
    public abstract class TestAbpController : AbpController
    {
        public TestAbpController()
        {
            LocalizationResource = typeof(BookStoreResource);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetValues(string id)
        {
            return Ok("TT");
        }
    }
}