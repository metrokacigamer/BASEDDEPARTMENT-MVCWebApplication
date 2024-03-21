using BASEDDEPARTMENT.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<MyDBContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("MyConString"))
															.UseLazyLoadingProxies());
			builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
				{
					options.Password.RequiredLength = 8;
				})
				.AddEntityFrameworkStores<MyDBContext>()
				.AddDefaultTokenProviders();

			builder.Services.ConfigureServices();

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

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
