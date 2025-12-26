using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Shared
{
    public static class Permissions
    {
        // User Management
        public const string UserView = "USER_VIEW";
        public const string UserCreate = "USER_CREATE";
        public const string UserEdit = "USER_EDIT";
        public const string UserDelete = "USER_DELETE";

        // Role Management
        public const string RoleView = "ROLE_VIEW";
        public const string RoleCreate = "ROLE_CREATE";
        public const string RoleEdit = "ROLE_EDIT";
        public const string RoleDelete = "ROLE_DELETE";

        // Product Type Management
        public const string ProductTypeView = "PRODUCT_TYPE_VIEW";
        public const string ProductTypeCreate = "PRODUCT_TYPE_CREATE";
        public const string ProductTypeEdit = "PRODUCT_TYPE_EDIT";
        public const string RProductTypeDelete = "PRODUCT_TYPE_DELETE";

        public const string PermissionView = "PERMISSION_VIEW";
    }
}
