using KMCSCI3110Project.Data;
using KMCSCI3110Project.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleFeatureRepository, VehicleFeatureRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // enable role management
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles and ensure admin account has "Admin" role.
using (var scope = app.Services.CreateScope())
{
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    const string adminRole = "Admin";
    const string adminEmail = "admin@kmcars.net";

    // Create the Admin role if it doesn't exist
    if (!await roleMgr.RoleExistsAsync(adminRole))
    {
        await roleMgr.CreateAsync(new IdentityRole(adminRole));
    }

    // Find the user by email
    var adminUser = await userMgr.FindByEmailAsync(adminEmail);
    if (adminUser != null)
    {
        // Add the user to the Admin role if not already
        if (!await userMgr.IsInRoleAsync(adminUser, adminRole))
        {
            await userMgr.AddToRoleAsync(adminUser, adminRole);
        }
    }
    else
    {
        // Create the user if they don't exist
        var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userMgr.CreateAsync(newAdmin, "Password1!");
        if (result.Succeeded)
        {
            await userMgr.AddToRoleAsync(newAdmin, adminRole);
        }
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
