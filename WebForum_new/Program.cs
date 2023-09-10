using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebForum_new.Authorization.Handlers;
using WebForum_new.Authorization.Handlers.PostVote;
using WebForum_new.Authorization.Requirements;
using WebForum_new.Authorization.Requirements.PostVote;
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

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();


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

await ConfigureAndRunApp(builder);


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

    options.AddPolicy(
        "CanDislikePost",
        policyBuilder => policyBuilder
            .AddRequirements(new CanDislikePostRequirement())
    );
}

void RegisterAuthorizationHandlers(WebApplicationBuilder webBuilder)
{
    webBuilder.Services.AddScoped<IAuthorizationHandler, IsCommunityOwnerHandler>();
    webBuilder.Services.AddScoped<IAuthorizationHandler, IsCommunitySubscriberHandler>();
    webBuilder.Services.AddScoped<IAuthorizationHandler, CanLikePostHandler>();
    webBuilder.Services.AddScoped<IAuthorizationHandler, CanDislikePostHandler>();
}

async Task ConfigureAndRunApp(WebApplicationBuilder webBuilder)
{
    async Task ForDevelopementOnly(IApplicationBuilder webApplication)
    {
        bool seedData = false;
        if (seedData)
        {
            await Seed.SeedUsersAndRolesAsync(webApplication);
        }
    }

    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

    try
    {
        Log.Information("Starting Web Application");

        var app = webBuilder.Build();

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

        await ForDevelopementOnly(app);
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Application terminated unexpectedly");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}