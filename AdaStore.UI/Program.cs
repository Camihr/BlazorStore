using AdaStore.Shared.Conts;
using AdaStore.Shared.Data;
using AdaStore.Shared.Enums;
using AdaStore.Shared.Models;
using AdaStore.UI.Interfaces;
using AdaStore.UI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-CO");

builder.Services.AddDbContextFactory<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient(p => p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddDefaultTokenProviders()
.AddDefaultUI()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var scope = app.Services.CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

if (!await roleManager.RoleExistsAsync(Conts.Admin))
    await roleManager.CreateAsync(new IdentityRole<int>(Conts.Admin));

if (!await roleManager.RoleExistsAsync(Conts.Buyer))
    await roleManager.CreateAsync(new IdentityRole<int>(Conts.Buyer));

if (await userManager.FindByEmailAsync("admin@adastore.co") == null)
{
    var user = new User
    {
        UserName = "admin@adastore.co",
        Email = "admin@adastore.co",
        Name = "Admin",
        Address = string.Empty,
        PhoneNumber = string.Empty,
        Document = string.Empty,

        Profile = Profiles.Admin,
    };

    var result = await userManager.CreateAsync(user, "1234567");

    if (result.Succeeded)
        await userManager.AddToRoleAsync(user, Conts.Admin);
}

app.Run();
