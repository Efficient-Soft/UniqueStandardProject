using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickFoodInfrastructure.Logging;
using QuickFoodWeb.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UniqueStandardProject.Data;
using UniqueStandardProject.Interfaces;
using UniqueStandardProject.Services;

namespace UniqueStandardProject
{
    public class Startup
    {
        private readonly string JsonPath = Path.Combine(Directory.GetCurrentDirectory(), "permission.json");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<UniqueStandardDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
           
            services.AddRazorPages();

            services.AddTransient<IFileService, FileService>();
            // Add Infrastructure Layer
            services.AddTransient(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            // Authentication Service
            services
                .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            // TODO: Permision Razor Pages
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //services.AddRazorPages(ConventionsAuthorizationPolicy).AddRazorRuntimeCompilation();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/Index";
                options.AccessDeniedPath = "/accessDenied";
                options.SlidingExpiration = true;
            });

            // UserManager & RoleManager require logging and HttpContext dependencies
            services.AddLogging();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //add api
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "api_default",
                  pattern: "api/{controller=Home}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "UserManage",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "Categories",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "Products",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            
        }

        /// <summary>
        /// Authorization Policy Services
        /// </summary>
        /// <param name="services"></param>
        public void AuthorizationPolicyService(IServiceCollection services)
        {
            List<PermissionModel> permissions = JsonSerializer.Deserialize<List<PermissionModel>>(File.ReadAllText(JsonPath));
            permissions.ForEach(permission =>
            {
                permission.Policies.ForEach(policy =>
                {
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy(policy.Policy, p => p.RequireClaim(policy.Claim_Type, "Access"));
                    });
                });
            });
        }

    }
}
