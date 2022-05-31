using Volo.Abp.Settings;

namespace Acme.Books.Settings
{
    public class BooksSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BooksSettings.MySetting1));
        }
    }
}
