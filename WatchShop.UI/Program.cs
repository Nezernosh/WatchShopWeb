using Microsoft.AspNetCore.Authentication.Cookies;
using WatchShop.BLL;
using WatchShop.BLL.Interfaces;
using WatchShop.DAL;
using WatchShop.DAL.Interfaces;
using WatchShop.TelegramBot;

var builder = WebApplication.CreateBuilder(args);

/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<DefaultDbContext>(options => options.UseSqlServer(connectionString));*/

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Users/Login";
    options.AccessDeniedPath = "/Users/Login";
    options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
});

builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllersWithViews();

// Add scoped service in Program.cs
builder.Services.AddScoped<IUsersDAL, UsersDAL>();
var t = builder.Services.AddTransient<IUsersBLL, UsersBLL>();
builder.Services.AddScoped<IWatchesDAL, WatchesDAL>();
builder.Services.AddScoped<IWatchesBLL, WatchesBLL>();
builder.Services.AddScoped<TGBot>();
ServiceProvider serviceProvider = t.BuildServiceProvider();
TGBot tg = new TGBot(serviceProvider.GetService<IUsersBLL>());

/*builder.Services.AddSingleton<RabbitMQClient>();
RabbitMQClient rmc = new RabbitMQClient();
builder.Services.AddSingleton<RedisStorageClient>();
RedisStorageClient rsc = new RedisStorageClient();*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
