using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
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
				.AddFacebook(options =>
				{
					options.AppId = "671603451675659";
					options.AppSecret = "58bb15b0c46102d335567f04f6fa573a";
				})
				.AddGoogle(options =>
				{
					options.ClientId = "63957170644-vak7jk9aofrvcnkfu5dtnirk3rhfl612.apps.googleusercontent.com";
					options.ClientSecret = "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";
				})
				.AddOAuth("github", "GitHub", option =>
				{

					option.ClientId = "0a681248a0ac1c44e8d1";
					option.ClientSecret = "4e8231cd879d6041200637592851b9b673e96f41";
					option.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

					option.TokenEndpoint = "https://github.com/login/oauth/access_token";
					option.UserInformationEndpoint = "https://api.github.com/user";
					option.CallbackPath = "/signin-github";
					option.ClaimActions.MapJsonKey("sub", "id");
					option.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
					option.Events.OnCreatingTicket = async http =>
					{
						using var request = new HttpRequestMessage(HttpMethod.Get, http.Options.UserInformationEndpoint);
						request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", http.AccessToken);
						using var result = await http.Backchannel.SendAsync(request);
						var user = await result.Content.ReadFromJsonAsync<JsonElement>();
						http.RunClaimActions(user);

					};

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
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}