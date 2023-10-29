using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyLogin.Data;
using MyLogin.Domain.IdentityEntities;
 
using Microsoft.EntityFrameworkCore;
using MyLogin.Middleware;
using Microsoft.AspNetCore.Http;

namespace  StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration) {

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<MyCustomMiddleware>();
            services.AddTransient<DisableBackButtonAfterLogoutMiddleware>();

            //enable identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<ApplicationDbContext>()

                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>() //repository layer for user table
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();// repositoey layer for role table

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("NotAuthorized", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !
                        context.User.Identity.IsAuthenticated;
                    });
                });
            });
        
        services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            return services;
        }
    }
}
