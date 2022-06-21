using System;
using System.Collections.Generic;
using System.Linq;
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
    public class BooksAppService : ApplicationService, IBooksAppService
    {
        private readonly IRepository<Book, Guid> repository;
        private readonly IRepository<BookOnly, Guid> onlyrepository;
        private readonly IRepository<Auth, Guid> authRepository;
        private readonly IBookService bookService;
        public BooksAppService(IRepository<Book, Guid> repository, IRepository<Auth, Guid> authRepository, IRepository<BookOnly, Guid> onlyrepository,
            IBookService bookService)
        {
            LocalizationResource = typeof(BooksResource);
            this.repository = repository;
            this.authRepository = authRepository;
            this.onlyrepository = onlyrepository;
            this.bookService = bookService;
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
            var book0 = new Book
            {
                Name = input.Name + "1111111"
            };

            try
            {
                await repository.InsertAsync(book0, autoSave: true); // 事务内提交

                //var query0 = await repository.FirstOrDefaultAsync(x => x.Name == book0.Name); // 同事务内可查

                //using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                //{

                //    var query00 = await repository.FirstOrDefaultAsync(x => x.Name == book0.Name); // 事务隔离，查不到

                //    var book = new Book
                //    {
                //        Name = input.Name
                //    };

                //    await repository.InsertAsync(book); // 未提交
                //    var query1 = await repository.FirstOrDefaultAsync(x => x.Name == input.Name); // 查不到
                //    var book11 = new Book
                //    {
                //        Name = input.Name
                //    };

                //    await repository.InsertAsync(book11,autoSave:true); // 事务内提交
                //    var ss = "ssssssssss";
                //    var query2 = await repository.FirstOrDefaultAsync(x => x.Name == book.Name); // 同事务内可查到
                //    await uow.CompleteAsync(); // 事务提交，其他session可查
                //    await uow.RollbackAsync(); // 事务提交之后回滚无效
                //}
                //using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                //{
                //    var temp = CurrentUnitOfWork; // 当前事务 uow
                //    var book2 = new Book
                //    {
                //        Name = input.Name + "2222"
                //    };

                //    await repository.InsertAsync(book2, autoSave:true); // 事务内提交
                //    var ss = "ssssssssss";
                //    var query3 = await repository.FirstOrDefaultAsync(x => x.Name == book2.Name); // 同事务内可查到
                //    await uow.RollbackAsync(); // 事务回滚
                //}
                //using (var uow = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                //{
                //    var book3 = new CreateBookQuery
                //    {
                //        Name = input.Name + "333",
                //        Publisher = "qq",
                //        Auths = new List<CreateAuthQuery>(),
                //    };

                //    await CreateBookSencodeAsync(book3);
                //    var ss = "ssssssssss";
                //    var query3 = await repository.FirstOrDefaultAsync(x => x.Name == book3.Name);
                //    await uow.RollbackAsync(); // 事务回滚
                //}
                //using (var uow4 = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                //{
                //    var book4 = new CreateBookQuery
                //    {
                //        Name = input.Name + "444",
                //        Publisher = "qq",
                //        Auths = new List<CreateAuthQuery>(),
                //    };

                //    await bookService.CreateBooksAsync(book4);
                //    var ss = "ssssssssss";
                //    var query3 = await repository.FirstOrDefaultAsync(x => x.Name == book4.Name);
                //    await uow4.RollbackAsync(); // 事务回滚
                //}
                //var bklist = NewBookList();
                //foreach (var item in bklist)
                //{
                //    using (var uow5 = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                //    {
                //        try
                //        {
                //            await bookService.CreateBooksAsync(item);
                //            var ss = "ssssssssss";
                //            var query3 = await repository.FirstOrDefaultAsync(x => x.Name == item.Name);
                //            await uow5.CompleteAsync();
                //        }
                //        catch
                //        {
                //            await uow5.RollbackAsync(); // 事务回滚
                //        }
                //    }
                //}

                var bkAlllist = await GetBookList();
                var dd = "ssssssssss";
                foreach (var item in bkAlllist)
                {
                    using (var uow5 = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
                    {
                        try
                        {
                            await bookService.ChangeBooksAsync(item);
                            var ss = "ssssssssss";
                            var query3 = await repository.FirstOrDefaultAsync(x => x.Name == item.Name);
                            await uow5.CompleteAsync();
                        }
                        catch (Exception ee)
                        {
                            await uow5.RollbackAsync(); // 事务回滚
                        }
                    }
                }

            }
            catch (Exception ex)
            { 
            }

            // await bookService.CreateBooksAsync(input);

            var query4 = await repository.FirstOrDefaultAsync(x => x.Name == book0.Name); // 之前已提交，可查
            // await CurrentUnitOfWork.RollbackAsync(); // 事务回滚
            var query5 = await repository.FirstOrDefaultAsync(x => x.Name == book0.Name); // 回滚之后不可查
            return L["LongWelcomeMessage"];
        }

        public async Task<string> CreateBookSencodeAsync(CreateBookQuery input)
        {
            try
            {
                var temp = CurrentUnitOfWork; // 与调用方上下文为同一事务
                var book = new Book
                {
                    Name = input.Name
                };
                await repository.InsertAsync(book, autoSave: true);
                var query = await repository.FirstOrDefaultAsync(x => x.Name == book.Name); // 同事务内可查到
            }
            catch (Exception ex)
            {
            }

            return L["LongWelcomeMessage"];
        }

        public async Task<string> CreateBookOnlysAsync(CreateBookQuery input)
        {
            try
            {
                var book = new BookOnly
                {
                    Name = input.Name
                };
                await onlyrepository.InsertAsync(book, autoSave:true);
            }
            catch (Exception ex)
            {
            }

            return L["LongWelcomeMessage"];
        }

        public async Task<string> CreateAuthsAsync(CreateAuthQuery input)
        {
            try
            {
                var a = new Auth
                {
                    Name = input.Name
                };

                await authRepository.InsertAsync(a);
            }
            catch (Exception ex)
            { 
            }

            return L["LongWelcomeMessage"];
        }

        private List<CreateBookQuery> NewBookList()
        {
            return new List<CreateBookQuery>
            {
                new CreateBookQuery
                    {
                        Name = "bk001",
                        Publisher = "1",
                    },
                new CreateBookQuery
                    {
                        Name = "bk002",
                        Publisher = "EX",
                    },
                new CreateBookQuery
                    {
                        Name = "bk003",
                        Publisher = "1",
                    },
            };
        }

        private  Task<List<Book>> GetBookList()
        {
            return repository.ToListAsync();
        }
    }
}
