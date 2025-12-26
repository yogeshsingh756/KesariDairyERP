using Microsoft.AspNetCore.Authorization;

namespace KesariDairyERP.Api.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
