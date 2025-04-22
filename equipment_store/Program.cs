using equipment_store.Areas.Admin.Repository;
using equipment_store.Models;
using equipment_store.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//connection db
builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]); });

//Add email sender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30);options.Cookie.IsEssential = true; });

//Identity
builder.Services.AddIdentity<AppUserModel,IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	// User settings.
	options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Category",
	pattern: "/Category/{slug?}",
    defaults: new { controller = "Category", action = "Index", });


app.MapControllerRoute(
	name: "Brand",
	pattern: "/Brand/{slug?}",
	defaults: new { controller = "Brand", action = "Index", });



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





//seeding data
var context =app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
