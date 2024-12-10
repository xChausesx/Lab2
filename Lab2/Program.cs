using Lab2.Context;
using Microsoft.EntityFrameworkCore;
using Lab2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using static System.Formats.Asn1.AsnWriter;
using Lab2.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_LAB2")), 
    ServiceLifetime.Transient);

builder.Services.AddDefaultIdentity<AppUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
});

using var scope = builder.Services.BuildServiceProvider().CreateScope();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
{
    if (!await roleManager.RoleExistsAsync(role.ToString()))
    {
        await roleManager.CreateAsync(new IdentityRole(role.ToString()));
    }
}

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.MapRazorPages();

await app.RunAsync();
