using Acme.BookStore.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.BookStore.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class BookStorePageModelBase : AbpPageModel
    {
        protected BookStorePageModelBase()
        {
            LocalizationResourceType = typeof(BookStoreResource);
        }
    }
}