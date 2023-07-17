using Microsoft.EntityFrameworkCore;
using TestProject.Areas.Identity.Data;
using TestProject.Data;
namespace TestProject
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

			builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

			builder.Services.AddAuthentication()
				.AddGoogle(options =>
				{
					options.ClientId = "63957170644-vak7jk9aofrvcnkfu5dtnirk3rhfl612.apps.googleusercontent.com";
					options.ClientSecret = "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";
				})
				.AddGitHub(option =>
				{
					option.ClientId = "0a681248a0ac1c44e8d1";
					option.ClientSecret = "4e8231cd879d6041200637592851b9b673e96f41";
					option.Scope.Add("user:email");
				})
				.AddFacebook(options =>
				{
					options.AppId = "1001272497558868";
					options.AppSecret = "2470792f896cb8e63f2c1d94f4d54a27";
				});


			// Add services to the container.
			builder.Services.AddControllersWithViews();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();


			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id:int?}");

			app.Run();
		}
	}
}