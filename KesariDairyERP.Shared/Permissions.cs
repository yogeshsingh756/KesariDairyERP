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

        // Ingredient Type Management
        public const string IngredientTypeView = "INGREDIENT_TYPE_VIEW";
        public const string IngredientTypeCreate = "INGREDIENT_TYPE_CREATE";
        public const string IngredientTypeEdit = "INGREDIENT_TYPE_EDIT";
        public const string IngredientTypeDelete = "INGREDIENT_TYPE_DELETE";

        // Production Batch Management
        public const string ProductionBatchView = "PRODUCTION_BATCH_VIEW";
        public const string ProductionBatchCreate = "PRODUCTION_BATCH_CREATE";

        public const string PermissionView = "PERMISSION_VIEW";
        public const string DashboardView = "DASHBOARD_VIEW";

        // Purchases Management
        public const string PurchasesView = "PURCHASE_VIEW";
        public const string PurchasesCreate = "PURCHASE_CREATE";

        // Vendors Management

        public const string VendorsView = "VENDORS_VIEW";
        public const string VendorsCreate = "VENDORS_CREATE";
        public const string VendorsEdit = "VENDORS_EDIT";
        public const string VendorsDelete = "VENDORS_DELETE";

        // Inventory Management
        public const string InventoryView = "INVENTORY_VIEW";

        // Vendors Ledgers Management
        public const string VendorsLedgersView = "VENDORS_LEDGERS_VIEW";

    }
}
