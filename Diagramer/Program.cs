using System.Security.AccessControl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Diagramer.Data;
using Diagramer.Hubs;
using Diagramer.Models.Identity;
using Diagramer.Services;

void InitRoles(WebApplicationBuilder builder, WebApplication app)
{
    using var serviceScope = app.Services.CreateScope();
    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    var roles = builder.Configuration.GetSection("Roles").Get<List<string>>();
    var rolesDB = context.Roles.ToList();
    foreach (var role in roles)
    {
        var roleDB = rolesDB.FirstOrDefault(r => r.Name.ToLower() == role.ToLower());
        if (roleDB == null)
        {
            context.Roles.Add(new IdentityRole<Guid>
            {
                Name = role,
                NormalizedName = role.ToUpper()
            });
        }
    }
    context.SaveChanges();
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiagrammerService, DiagrammerService>();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7058")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});
var app = builder.Build();
InitRoles(builder, app);
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
app.UseCors();
app.MapHub<DiagrammerHub>("/diagrammerHub");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();