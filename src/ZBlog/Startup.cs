using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZBlog.Models;
using ZBlog.Services;

namespace ZBlog
{
    public class Startup
    {
        private readonly Platform _platform;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (hostingEnvironment.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            
            Configuration = builder.Build();

            _platform = new Platform();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddDbContext<ZBlogDbContext>(options =>
                options.UseSqlite(Configuration["Data:DefaultConnection:ConnectionString"]));
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://zhangmm.cn");
                });
            });

            services.AddMvc();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(20));

            services.AddSingleton(Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Configure Auth
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ManageZBlog",
                    authBuilder => {
                        authBuilder.RequireClaim("ManageZBlog", "Allowed");
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (hostingEnvironment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseRuntimeInfoPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                /*// For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ZBlogDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }*/
            }
            
            app.UseStaticFiles();

            app.UseSession();
            
            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });

            SampleData.InitializeZBlog(app.ApplicationServices).Wait();
        }
    }
}
