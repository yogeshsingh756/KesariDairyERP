using KesariDairyERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<ProductType> ProductType => Set<ProductType>();
        public DbSet<IngredientType> IngredientType => Set<IngredientType>();
        public DbSet<ProductionBatch> ProductionBatch => Set<ProductionBatch>();
        public DbSet<ProductionBatchIngredient> ProductionBatchIngredient => Set<ProductionBatchIngredient>();
        public DbSet<InventoryStock> InventoryStock => Set<InventoryStock>();
        public DbSet<InventoryTransaction> InventoryTransaction => Set<InventoryTransaction>();
        public DbSet<PurchaseItem> PurchaseItem => Set<PurchaseItem>();
        public DbSet<PurchaseMaster> PurchaseMaster => Set<PurchaseMaster>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureRole(modelBuilder);
            ConfigurePermission(modelBuilder);
            ConfigureUserRole(modelBuilder);
            ConfigureRolePermission(modelBuilder);
            ConfigureProductionBatch(modelBuilder);          
            ConfigureProductionBatchIngredient(modelBuilder);
            ConfigureInventoryStock(modelBuilder);
            ConfigureInventoryTransaction(modelBuilder);
            ConfigurePurchaseMaster(modelBuilder);
            ConfigurePurchaseItem(modelBuilder);

        }
        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
                entity.Property(x => x.Username).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
                entity.Property(x => x.Gender).HasMaxLength(20);
                entity.Property(x => x.Address).HasColumnType("TEXT");
                entity.Property(x => x.PasswordHash).IsRequired();

                entity.HasIndex(x => x.Username).IsUnique();
                entity.HasIndex(x => x.Email).IsUnique();

                entity.HasOne(x => x.UserRole)
                      .WithOne(x => x.User)
                      .HasForeignKey<UserRole>(x => x.UserId);
            });
        }
        private static void ConfigureRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.RoleName).HasMaxLength(100).IsRequired();
                entity.HasIndex(x => x.RoleName).IsUnique();
            });
        }
        private static void ConfigurePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.PermissionKey).HasMaxLength(150).IsRequired();
                entity.Property(x => x.PermissionName).HasMaxLength(150).IsRequired();
                entity.Property(x => x.ModuleName).HasMaxLength(100).IsRequired();

                entity.HasIndex(x => x.PermissionKey).IsUnique();
            });
        }

        private static void ConfigureUserRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");

                entity.HasKey(x => x.UserId); // Enforces one role per user

                entity.HasOne(x => x.User)
                      .WithOne(x => x.UserRole)
                      .HasForeignKey<UserRole>(x => x.UserId);

                entity.HasOne(x => x.Role)
                      .WithMany()
                      .HasForeignKey(x => x.RoleId);
            });
        }
        private static void ConfigureRolePermission(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permissions");

                entity.HasKey(x => new { x.RoleId, x.PermissionId });

                entity.HasOne(x => x.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(x => x.RoleId);

                entity.HasOne(x => x.Permission)
                      .WithMany()
                      .HasForeignKey(x => x.PermissionId);
            });
        }
        private static void ConfigureProductionBatch(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionBatch>(entity =>
            {
                entity.ToTable("production_batches");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.BatchUnit)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.HasOne(x => x.Product)
                      .WithMany()
                      .HasForeignKey(x => x.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Ingredients)
                      .WithOne(x => x.ProductionBatch)
                      .HasForeignKey(x => x.ProductionBatchId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static void ConfigureProductionBatchIngredient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionBatchIngredient>(entity =>
            {
                entity.ToTable("production_batch_ingredients");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Unit).HasMaxLength(20).IsRequired();

                entity.HasOne(x => x.IngredientType)
                      .WithMany()
                      .HasForeignKey(x => x.IngredientTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static void ConfigureInventoryStock(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryStock>(entity =>
            {
                entity.ToTable("inventory_stock");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.RawMaterialType)
                      .HasMaxLength(100)
                      .IsRequired();
            });
        }
        private static void ConfigureInventoryTransaction(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.ToTable("inventory_transactions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.RawMaterialType)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.TransactionType)
                      .HasMaxLength(50)
                      .IsRequired();
            });
        }
        private static void ConfigurePurchaseMaster(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseMaster>(entity =>
            {
                entity.ToTable("purchase_master");

                entity.HasKey(x => x.Id);

                entity.HasMany(x => x.Items)
                      .WithOne(x => x.PurchaseMaster)
                      .HasForeignKey(x => x.PurchaseMasterId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
        private static void ConfigurePurchaseItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.ToTable("purchase_items");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.RawMaterialType)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.Unit)
                      .HasMaxLength(20)
                      .IsRequired();
            });
        }



    }
}
