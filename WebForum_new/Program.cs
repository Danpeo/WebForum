using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebForum_new.Authorization.Handlers;
using WebForum_new.Authorization.Requirements;
using WebForum_new.Data;
using WebForum_new.Data.Settings;
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

RegisterAuthorizationHandlers(builder);


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

void AddCustomServices(WebApplicationBuilder webBuilder)
{
    webBuilder.Services.AddScoped<IAccountService, AccountService>();
    webBuilder.Services.AddScoped<IUserProfileService, UserProfileService>();
    webBuilder.Services.AddScoped<ICommunityService, CommunityService>();
    webBuilder.Services.AddScoped<IPostService, PostService>();
    webBuilder.Services.AddScoped<ICommentService, CommentService>();
    webBuilder.Services.AddScoped<IImageService, ImageService>();

    webBuilder.Services.AddScoped<ValidateModelAttribute>();
}

void AddDbConnection(WebApplicationBuilder webBuilder)
{
    string connection = webBuilder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    webBuilder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connection); });
}

async Task ForDevelopementOnly(WebApplication webApplication)
{
    bool seedData = false;
    if (seedData)
    {
        await Seed.SeedUsersAndRolesAsync(webApplication);
    }
}

void AddTagHelpers(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddTransient<TruncateTextTagHelper>();
    webApplicationBuilder.Services.AddTransient<ForLoggedTagHelper>();
    webApplicationBuilder.Services.AddTransient<ForNotLoggedTagHelper>();
}

void AddAppSettings(WebApplicationBuilder webAppBuilder)
{
    webAppBuilder.Services.Configure<AppDisplaySettings>(webAppBuilder.Configuration
        .GetSection("AppDisplaySettings"));

    webAppBuilder.Services.Configure<CloudinarySettings>(webAppBuilder.Configuration
        .GetSection("CloudinarySettings"));
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

    options.AddPolicy(
        "CanLikePost",
        policyBuilder => policyBuilder
            .AddRequirements(new CanLikePostRequirement())
    );
}

void RegisterAuthorizationHandlers(WebApplicationBuilder webBuilder)
{
    webBuilder.Services.AddScoped<IAuthorizationHandler, IsCommunityOwnerHandler>();
    webBuilder.Services.AddScoped<IAuthorizationHandler, IsCommunitySubscriberHandler>();
    webBuilder.Services.AddScoped<IAuthorizationHandler, CanLikePostHandler>();
}