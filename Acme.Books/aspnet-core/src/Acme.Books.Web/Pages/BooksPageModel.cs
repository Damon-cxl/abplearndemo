using Acme.Books.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.Books.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class BooksPageModel : AbpPageModel
    {
        protected BooksPageModel()
        {
            LocalizationResourceType = typeof(BooksResource);
        }
    }
}