using equipment_store.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//connection db
builder.Services.AddDbContext<DataContext>(options => { options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]); });


// Add services to the container.
builder.Services.AddControllersWithViews();

//Session

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(30);options.Cookie.IsEssential = true; });


var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//seeding data
var context=app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
SeedData.SeedingData(context);

app.Run();
