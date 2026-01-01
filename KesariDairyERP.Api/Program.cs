using KesariDairyERP.Api.Authorization;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Application.Services;
using KesariDairyERP.Infrastructure.Data;
using KesariDairyERP.Infrastructure.Repositories;
using KesariDairyERP.Infrastructure.Seed;
using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// fix for render
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
    serverOptions.ListenAnyIP(int.Parse(port));
});
// Add services to the container.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.UserView,
        policy => policy.Requirements.Add(
            new PermissionRequirement(Permissions.UserView)));

    options.AddPolicy(Permissions.UserCreate,
        policy => policy.Requirements.Add(
            new PermissionRequirement(Permissions.UserCreate)));

    options.AddPolicy(Permissions.UserEdit,
        policy => policy.Requirements.Add(
            new PermissionRequirement(Permissions.UserEdit)));

    options.AddPolicy(Permissions.UserDelete,
        policy => policy.Requirements.Add(
            new PermissionRequirement(Permissions.UserDelete)));
    options.AddPolicy(Permissions.RoleView,
    policy => policy.Requirements.Add(
        new PermissionRequirement(Permissions.RoleView)));
    options.AddPolicy(Permissions.RoleCreate,
policy => policy.Requirements.Add(
    new PermissionRequirement(Permissions.RoleCreate)));
    options.AddPolicy(Permissions.RoleEdit,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.RoleEdit)));
    options.AddPolicy(Permissions.RoleDelete,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.RoleDelete)));
    options.AddPolicy(Permissions.PermissionView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.PermissionView)));
    options.AddPolicy(Permissions.ProductTypeCreate,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductTypeCreate)));
    options.AddPolicy(Permissions.ProductTypeEdit,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductTypeEdit)));
    options.AddPolicy(Permissions.RProductTypeDelete,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.RProductTypeDelete)));
    options.AddPolicy(Permissions.ProductTypeView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductTypeView)));
    options.AddPolicy(Permissions.IngredientTypeCreate,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.IngredientTypeCreate)));
    options.AddPolicy(Permissions.IngredientTypeView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.IngredientTypeView)));
    options.AddPolicy(Permissions.IngredientTypeEdit,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.IngredientTypeEdit)));
    options.AddPolicy(Permissions.IngredientTypeDelete,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.IngredientTypeDelete)));


    options.AddPolicy(Permissions.ProductionBatchView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductionBatchView)));
    options.AddPolicy(Permissions.ProductionBatchCreate,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductionBatchCreate)));

    options.AddPolicy(Permissions.ProductionBatchDelete,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductionBatchDelete)));
    options.AddPolicy(Permissions.ProductionBatchEdit,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.ProductionBatchEdit)));

    options.AddPolicy(Permissions.DashboardView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.DashboardView)));

    options.AddPolicy(Permissions.PurchasesCreate,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.PurchasesCreate)));
    options.AddPolicy(Permissions.PurchasesView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.PurchasesView)));

    options.AddPolicy(Permissions.VendorsCreate,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.VendorsCreate)));
    options.AddPolicy(Permissions.VendorsDelete,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.VendorsDelete)));
    options.AddPolicy(Permissions.VendorsEdit,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.VendorsEdit)));
    options.AddPolicy(Permissions.VendorsView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.VendorsView)));

    options.AddPolicy(Permissions.InventoryView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.InventoryView)));

    options.AddPolicy(Permissions.VendorsLedgersView,
policy => policy.Requirements.Add(
new PermissionRequirement(Permissions.VendorsLedgersView)));

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );
});
builder.Services.AddScoped<IProductionBatchRepository, ProductionBatchRepository>();
builder.Services.AddScoped<IProductionBatchService, ProductionBatchService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
builder.Services.AddScoped<IIngredientTypeRepository, IngredientTypeRepository>();
builder.Services.AddScoped<IIngredientTypeService, IngredientTypeService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();
builder.Services.AddScoped<IRawMaterialService, RawMaterialService>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorLedgerRepository, VendorLedgerRepository>();
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IVendorLedgerService, VendorLedgerService>();
builder.Services.AddScoped<IBatchPackagingRepository, BatchPackagingRepository>();
builder.Services.AddScoped<IFinishedProductStockRepository, FinishedProductStockRepository>();
builder.Services.AddScoped<IBatchPackagingService, BatchPackagingService>();






builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Kesari Dairy ERP API",
        Version = "v1"
    });

    // JWT Authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins(
            "https://erp.kesari.biz",
            "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:5177"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();  // only if you send cookies/auth headers
    });
});

var app = builder.Build();
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    await RbacSeeder.SeedAsync(db);
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
