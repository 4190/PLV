using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using plv.Data;
using plv.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace plv
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public Dictionary<string, string> configDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("startupConfig.json"));
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = Convert.ToBoolean(configDictionary["passwordRequireDigit"]);
                options.Password.RequireLowercase = Convert.ToBoolean(configDictionary["passwordRequireLowercase"]); 
                options.Password.RequireNonAlphanumeric = Convert.ToBoolean(configDictionary["passwordRequireNonAlphanumeric"]); 
                options.Password.RequireUppercase = Convert.ToBoolean(configDictionary["passwordRequireUppercase"]); 
                options.Password.RequiredLength = Convert.ToInt32(configDictionary["passwordRequiredLength"]);
                options.Password.RequiredUniqueChars = Convert.ToInt32(configDictionary["passwordRequiredUniqueChars"]);

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(configDictionary["defaultLockoutTimeSpan"]));
                options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(configDictionary["maxFailedAccessAttempts"]);
                options.Lockout.AllowedForNewUsers = Convert.ToBoolean(configDictionary["lockoutAllowedForNewUsers"]);

                // User settings.
                options.User.AllowedUserNameCharacters = configDictionary["allowedUserNameCharacters"];
                options.User.RequireUniqueEmail = Convert.ToBoolean(configDictionary["requireUniqueEmail"]);
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            CreateRoles(services).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            //here in this line we are adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            var userRoleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                //here in this line we are creating admin role and seed it to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if(!userRoleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }

            //here we are assigning the Admin role to account specified in startupConfig.json
            //if user does not exist it will be created
            //When will we run this project then it will
            //be assigned to that user.
            

            if(await UserManager.FindByNameAsync(configDictionary["adminUserName"]) != null)
            {
                ApplicationUser user = await UserManager.FindByNameAsync("admin@gmail.com");
                await UserManager.AddToRoleAsync(user, "Admin");
            }
            
            else
            {
                ApplicationUser adminUser = new ApplicationUser()
                {
                    UserName = configDictionary["adminUserName"],
                    Email = configDictionary["adminUserEmail"]
                };
                await UserManager.CreateAsync(adminUser, configDictionary["adminPassword"]);
                await UserManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
