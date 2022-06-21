using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acme.Books.Localization;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Acme.Books
{
    /* Inherit your application services from this class.
     */
    public interface IBookService
    {
        string len();

        Task<string> GetTestAsync(CreateBookQuery input);

        Task<string> CreateBooksAsync(CreateBookQuery input);

        Task<string> ChangeBooksAsync(Book input);

        Task<List<Book>> CreateBookTestNullAsync(string author, List<string> names);
    }
}
