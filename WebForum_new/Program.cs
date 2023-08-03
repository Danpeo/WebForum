using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Data;
using WebForum_new.Filters;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.TagHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ValidateModelAttribute>();

string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connection); });

builder.Services.AddIdentity<AppUser, IdentityRole>(options => { options.Password.RequiredLength = 8; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<AppDisplaySettings>(builder.Configuration
    .GetSection("AppDisplaySettings"));


builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

#region TagHelpers

builder.Services.AddTransient<TruncateTextTagHelper>();
builder.Services.AddTransient<ForLoggedTagHelper>();

#endregion

var app = builder.Build();

#region Developement

bool seedData = true;
if (seedData)
{
    await Seed.SeedUsersAndRolesAsync(app);
}

#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();