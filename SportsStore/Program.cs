using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using SportsStore.Models;

var builder = WebApplication.CreateBuilder (
		args : args
	);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext> (
		optionsAction : opts =>
		{
			opts.UseSqlServer (
					connectionString : builder.Configuration[key : "ConnectionStrings:SportsStoreConnection"]
				);
		}
	);

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart> (
		implementationFactory : sp => SessionCart.GetCart (
				services : sp
			)
	);
builder.Services.AddSingleton<IHttpContextAccessor,
	HttpContextAccessor>();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppIdentityDbContext> (
		optionsAction : options =>
			options.UseSqlServer (
					connectionString : builder.Configuration[key : "ConnectionStrings:IdentityConnection"]
				)
	);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
	AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

if ( app.Environment.IsProduction() )
{
	app.UseExceptionHandler (
			errorHandlingPath : "/error"
		);
}

app.UseRequestLocalization (
		optionsAction : opts =>
		{
			opts.AddSupportedCultures (
						"en-US"
					).
				AddSupportedUICultures (
						"en-US"
					).
				SetDefaultCulture (
						defaultCulture : "en-US"
					);
		}
	);

app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute (
		name : "catpage",
		pattern : "{category}/Page{productPage:int}",
		defaults : new
				   {
					   Controller = "Home",
					   action = "Index"
				   }
	);

app.MapControllerRoute (
		name : "page",
		pattern : "Page{productPage:int}",
		defaults : new
				   {
					   Controller = "Home",
					   action = "Index",
					   productPage = 1
				   }
	);

app.MapControllerRoute (
		name : "category",
		pattern : "{category}",
		defaults : new
				   {
					   Controller = "Home",
					   action = "Index",
					   productPage = 1
				   }
	);

app.MapControllerRoute (
		name : "pagination",
		pattern : "Products/Page{productPage}",
		defaults : new
				   {
					   Controller = "Home",
					   action = "Index",
					   productPage = 1
				   }
	);

app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage (
		pattern : "/admin/{*catchall}",
		page : "/Admin/Index"
	);

SeedData.EnsurePopulated (
		app : app
	);
IdentitySeedData.EnsurePopulated (
		app : app
	);

app.Run();


// 321
// Inspect app week !
