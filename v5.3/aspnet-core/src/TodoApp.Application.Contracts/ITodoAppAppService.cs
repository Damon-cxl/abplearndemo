using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Localization;
using Volo.Abp.Application.Services;

namespace TodoApp;

/* Inherit your application services from this class.
 */
public interface ITodoAppAppService
{
    Task<string> CreateBooksAsync(string input);
}
