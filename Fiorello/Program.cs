using Fiorello.DAL;
using Fiorello.Models;
using Fiorello.Utilities;
using Fiorello.Utilities.EmailService.EmailSender;
using Fiorello.Utilities.EmailService.EmailSender.Abstract;
using Fiorello.Utilities.EmailService.EmailSender.Concrete;
using Fiorello.Utilities.File;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredUniqueChars = 3;
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = true;
	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedEmail = true;
})
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

var configuration = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(configuration);
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddSingleton<IFileService, FileService>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = context =>
	{
		if (context.HttpContext.Request.Path.Value.StartsWith("/admin") || context.HttpContext.Request.Path.Value.StartsWith("/Admin"))
		{
			var redirectPath = new Uri(context.RedirectUri);
			context.Response.Redirect("/admin/account/login" + redirectPath.Query);
		}
		else
		{
			var redirectPath = new Uri(context.RedirectUri);
			context.Response.Redirect("/account/login" + redirectPath.Query);
		}
		return Task.CompletedTask;
	};
});

var app = builder.Build();

app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapDefaultControllerRoute();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
	var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
	await DbInitializer.SeedAsync(roleManager, userManager);
}

app.Run();