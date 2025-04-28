using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectPRN222.Areas.Identity.SeedData;
using ProjectPRN222.Models;
using ProjectPRN222.Services;
using Twilio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();


builder.Services.AddDbContext<Prn222projectContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var accountSid = builder.Configuration["Twilio:AccountSid"];
var authToken = builder.Configuration["Twilio:AuthToken"];
TwilioClient.Init(accountSid, authToken);

builder.Services.AddSingleton<SmsService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddSingleton<BankSettings>();

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<Prn222projectContext>()
	.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 3;
	options.Lockout.AllowedForNewUsers = true;
});

builder.Services.AddRazorPages(options =>
{
	options.Conventions.AuthorizeFolder("/");
	options.Conventions.AllowAnonymousToPage("/Account/Login");
	options.Conventions.AllowAnonymousToPage("/Account/Register");
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<User>>();
	var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
	await SeedData.Initialize(services, userManager, roleManager);
}
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
app.UseSession();
app.UseAuthentication(); // ?? Xác th?c
app.UseAuthorization();  // ? Phân quy?n


app.UseStatusCodePages(async context =>
{
	var user = context.HttpContext.User;

	if (context.HttpContext.Response.StatusCode == 404)
	{
		if (user.Identity.IsAuthenticated)
		{
			context.HttpContext.Response.Redirect("/UserSite/Home");
		}
		else
		{
			context.HttpContext.Response.Redirect("/Identity/Account/Login");
		}
	}
});

app.MapRazorPages();

app.Run();
