using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WonderDevBlogMVC2024.Data;
using WonderDevBlogMVC2024.Data.Repositories.Interfaces;
using WonderDevBlogMVC2024.Data.Repositories;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.Google;
using AspNet.Security.OAuth.GitHub;
using WonderDevBlogMVC2024.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                              throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity configuration w/Roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
   //Use for email confirmation, password reset, etc.
    .AddDefaultTokenProviders();

// Add Google and GitHub Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    // Ensure the default challenge scheme is set
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
    .AddGoogle(GoogleDefaults.AuthenticationScheme,googleOptions =>
    {
        var ClientId = builder.Configuration["Authentication:Google:ClientId"];
        var ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("Google ClientId or ClientSecret is not set in the configuration.");
        }

        googleOptions.ClientId = ClientId;
        googleOptions.ClientSecret = ClientSecret;
    })
    .AddGitHub(githubOptions =>
    {
        var ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
        var ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];

        if (string.IsNullOrEmpty(ClientId) || string.IsNullOrEmpty(ClientSecret))
        {
            throw new Exception("GitHub ClientId or ClientSecret is not set in the configuration.");
        }

        githubOptions.ClientId = ClientId;
        githubOptions.ClientSecret = ClientSecret;
    });

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Add Razor components to enhance reusability
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents();

builder.Services.AddControllersWithViews();

//Register services, repositories and interfaces via the extension method
builder.Services.AddCustomServices(builder.Configuration);  

var app = builder.Build();

// Call DataService to seed the database
using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    // Call the SetupDB method to run migrations and seed roles/users
    await dataService.SetupDB(); 
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Register MVC route and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
