using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SOPCOVIDChecker.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SOPCOVIDChecker.Data;
using DinkToPdf.Contracts;
using DinkToPdf;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace SOPCOVIDChecker
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
            services.AddSingleton(Configuration);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<IFileProvider>(
            new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddDbContext<SOPCCContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SOPCCConnection")));

            services.AddCors();

            services.AddTransient<IUserService, UserService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/NotFound";
                options.ExpireTimeSpan = TimeSpan.FromHours(5);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RHUUsers", polBuilder => polBuilder.RequireClaim(ClaimTypes.Role, "RHU"));
                options.AddPolicy("PESUUsers", polBuilder => polBuilder.RequireClaim(ClaimTypes.Role, "PESU"));
                options.AddPolicy("RESUUsers", polBuilder => polBuilder.RequireClaim(ClaimTypes.Role, "RESU"));
                options.AddPolicy("LABUsers", polBuilder => polBuilder.RequireClaim(ClaimTypes.Role, "LAB"));
                options.AddPolicy("Admin", polBuilder => polBuilder.RequireClaim(ClaimTypes.Role, "admin"));
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });


            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            /*services.AddControllersWithViews().AddCon
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles(); 
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
