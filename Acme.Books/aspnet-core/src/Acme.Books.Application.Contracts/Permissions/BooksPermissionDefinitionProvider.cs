using Acme.Books.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Acme.Books.Permissions
{
    public class BooksPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BooksPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(BooksPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BooksResource>(name);
        }
    }
}
