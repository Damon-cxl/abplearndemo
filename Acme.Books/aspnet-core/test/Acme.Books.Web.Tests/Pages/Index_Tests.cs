using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Acme.Books.Pages
{
    public class Index_Tests : BooksWebTestBase
    {
        [Fact]
        public async Task Welcome_Page()
        {
            var response = await GetResponseAsStringAsync("/");
            response.ShouldNotBeNull();
        }
    }
}
