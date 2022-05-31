using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acme.Books.Localization;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Acme.Books
{
    /* Inherit your application services from this class.
     */
    public class BookService : ApplicationService, IBookService
    {
        private readonly IRepository<Book, Guid> repository;
        private readonly IRepository<BookOnly, Guid> onlyrepository;
        private readonly IRepository<Auth, Guid> authRepository;
        public BookService(IRepository<Book, Guid> repository, IRepository<Auth, Guid> authRepository, IRepository<BookOnly, Guid> onlyrepository)
        {
            LocalizationResource = typeof(BooksResource);
            this.repository = repository;
            this.authRepository = authRepository;
            this.onlyrepository = onlyrepository;
        }

        public string len()
        {
            return L["LongWelcomeMessage"];
        }

        public async Task<string> GetTestAsync(CreateBookQuery input)
        {
            if (input.Name == null)
            {
                throw new UserFriendlyException(L["GetSequenceFieldrew"]);
            }
            return L["LongWelcomeMessage"];
        }

        public async Task<string> CreateBooksAsync(CreateBookQuery input)
        {
            var temp = CurrentUnitOfWork; // 与调用方上下文为同一事务
            if (input.Publisher == "EX")
            {
                throw new UserFriendlyException("EX-publisher");
            }
            var book = new Book
            {
                Name = input.Name
            };
            await repository.InsertAsync(book, autoSave: true);
            var query = await repository.FirstOrDefaultAsync(x => x.Name == book.Name); // 同事务内可查到
            return L["LongWelcomeMessage"];
        }

        public async Task<string> ChangeBooksAsync(Book input)
        {
            var temp = CurrentUnitOfWork; // 与调用方上下文为同一事务
            var originname = input.Name;
            input.Name = "change";
            await repository.UpdateAsync(input, autoSave: true);
            if (originname.Contains("cc"))
            {
                throw new UserFriendlyException("cc");
            }
            return L["LongWelcomeMessage"];
        }
    }
}
