using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Acme.Books.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Acme.Books.Web.Pages
{
    /* Inherit your UI Pages from this class. To do that, add this line to your Pages (.cshtml files under the Page folder):
     * @inherits Acme.Books.Web.Pages.BooksPage
     */
    public abstract class BooksPage : AbpPage
    {
        [RazorInject]
        public IHtmlLocalizer<BooksResource> L { get; set; }
    }
}
