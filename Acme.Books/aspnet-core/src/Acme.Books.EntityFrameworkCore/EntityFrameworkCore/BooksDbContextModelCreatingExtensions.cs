using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Acme.Books.EntityFrameworkCore
{
    public static class BooksDbContextModelCreatingExtensions
    {
        public static void ConfigureBooks(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(BooksConsts.DbTablePrefix + "YourEntities", BooksConsts.DbSchema);

            //    //...
            //});

            builder.Entity<Book>(b =>
            {
                b.ToTable(BooksConsts.DbTablePrefix + "Books"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();

                /* Configure mappings for your additional properties
                 * Also see the BooksEfCoreEntityExtensionMappings class
                 */
            });

            builder.Entity<BookOnly>(b =>
            {
                b.ToTable(BooksConsts.DbTablePrefix + "BookOnlys"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();

                /* Configure mappings for your additional properties
                 * Also see the BooksEfCoreEntityExtensionMappings class
                 */
            });

            builder.Entity<Auth>(b =>
            {
                b.ToTable(BooksConsts.DbTablePrefix + "Auths"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();

                /* Configure mappings for your additional properties
                 * Also see the BooksEfCoreEntityExtensionMappings class
                 */
            });
        }
    }
}