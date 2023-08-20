using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Authorization.Handlers;
using WebForum_new.Authorization.Requirements;
using WebForum_new.Data;
using WebForum_new.Filters;
using WebForum_new.Models;
using WebForum_new.Services;
using WebForum_new.TagHelpers;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

AddCustomServices(builder);

AddDbConnection(builder);

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(RegisterPolicies);

builder.Services.AddScoped<IAuthorizationHandler, IsCommunityOwnerHandler>();


/*builder.Services.AddIdentity<AppUser, IdentityRole>(options => { options.Password.RequiredLength = 8; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()*/
;

AddAppSettings(builder);


/*builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();*/

AddTagHelpers(builder);

WebApplication app = builder.Build();

await ForDevelopementOnly(app);

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

void AddCustomServices(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddScoped<IAccountService, AccountService>();
    webApplicationBuilder.Services.AddScoped<IUserProfileService, UserProfileService>();
    webApplicationBuilder.Services.AddScoped<ICommunityService, CommunityService>();
    webApplicationBuilder.Services.AddScoped<IPostService, PostService>();
    webApplicationBuilder.Services.AddScoped<ICommentService, CommentService>();

    builder.Services.AddScoped<ValidateModelAttribute>();
}

void AddDbConnection(WebApplicationBuilder builder1)
{
    string connection = builder1.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    builder1.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connection); });
}

async Task ForDevelopementOnly(WebApplication webApplication)
{
    bool seedData = false;
    if (seedData)
    {
        await Seed.SeedUsersAndRolesAsync(webApplication);
    }
}

void AddTagHelpers(WebApplicationBuilder webApplicationBuilder1)
{
    webApplicationBuilder1.Services.AddTransient<TruncateTextTagHelper>();
    webApplicationBuilder1.Services.AddTransient<ForLoggedTagHelper>();
    webApplicationBuilder1.Services.AddTransient<ForNotLoggedTagHelper>();
}

void AddAppSettings(WebApplicationBuilder builder2)
{
    builder2.Services.Configure<AppDisplaySettings>(builder2.Configuration
        .GetSection("AppDisplaySettings"));
}

void RegisterPolicies(AuthorizationOptions options)
{
    options.AddPolicy(
        "CanManageCommunity",
        policyBuilder => policyBuilder
            .AddRequirements(new IsCommunityOwnerRequirement())
    );

    options.AddPolicy(
        "CommunitySubscriber",
        policyBuilder => policyBuilder
            .AddRequirements(new IsCommunitySubscriberRequirement())
    );
}