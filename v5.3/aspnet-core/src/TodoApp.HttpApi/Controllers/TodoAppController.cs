using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace TodoApp.Controllers;

/* Inherit your controllers from this class.
 */
[Route("api/v1/[controller]")]
public class TodoAppController : AbpControllerBase
{
    private ITodoAppAppService service;

    public TodoAppController(ITodoAppAppService service)
    {
        LocalizationResource = typeof(TodoAppResource);
        this.service = service;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<string> Create([FromBody] string input)
    {
        return await service.CreateBooksAsync(input);
    }
}
