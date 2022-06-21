using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Localization;
using Volo.Abp.Application.Services;

namespace TodoApp;

/* Inherit your application services from this class.
 */
public class TodoAppAppService : ApplicationService, ITodoAppAppService
{
    public TodoAppAppService()
    {
        LocalizationResource = typeof(TodoAppResource);
    }

    public async Task<string> CreateBooksAsync(string input)
    {
        return L["LongWelcomeMessage"];
    }
}
