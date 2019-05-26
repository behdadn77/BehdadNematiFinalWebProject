using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Areas.Identity.Data;
using Shop.Hubs;
using Shop.Models.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Shop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
            services.Configure<SitePropertiesViewModel>(Configuration.GetSection("SiteProperties"));
            services.Configure<AdminInfo>(Configuration.GetSection("SiteProperties").GetSection("admininfo"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider, IOptions<SitePropertiesViewModel> sitePropertiesViewModel)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for Production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication(); // added 4 authentication
            app.UseMvc(routes =>
            {
                routes.MapRoute(
           name: "areas",
           template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

               routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSignalR(r=>r.MapHub<ChatHub>("/ChatHub"));
            InitializeDatabaseAndIdentity(serviceProvider, sitePropertiesViewModel.Value).Wait();
        }
        public async Task InitializeDatabaseAndIdentity(IServiceProvider serviceProvider
            , SitePropertiesViewModel siteProperties)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var item in siteProperties.roles)
            {
                if (await roleManager.RoleExistsAsync(item) == false)
                {
                    IdentityRole newrole = new IdentityRole(item);
                    await roleManager.CreateAsync(newrole);
                }
            }

            var user = await userManager.FindByNameAsync(siteProperties.AdminInfo.adminname);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    Email = siteProperties.AdminInfo.adminname,
                    UserName = siteProperties.AdminInfo.adminname,
                    PhoneNumber = siteProperties.AdminInfo.adminphone,
                    FirstName = "",
                    LastName = ""
                };
                var status = await userManager.CreateAsync(user, siteProperties.AdminInfo.adminpassword);
                if (status.Succeeded == true)
                {
                    await userManager.AddToRoleAsync(user, "admins");
                }
            }
        }

    }
}
