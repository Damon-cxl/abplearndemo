using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Acme.Books.Web
{
    [Dependency(ReplaceServices = true)]
    public class BooksBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Books";
    }
}
