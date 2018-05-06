using System;
using System.Text;
using EnarcLabs.Technonomicon.Daemon.MessageService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// ReSharper disable UnusedMember.Global

namespace EnarcLabs.Technonomicon.Daemon
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Called during startup. Adds services to the container.
        /// </summary>
        /// <param name="services">This method will alter this collection to add the required services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(Configuration);

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlite("Data Source=users.sqlite",
                optionsBuilder => optionsBuilder.MigrationsAssembly("EnarcLabs.Technonomicon.Daemon")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;

                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })
                .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecretKey"))),

                        ValidateIssuer = false,
                        ValidIssuer = "",

                        ValidateAudience = false,
                        ValidAudience = "",

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });

            services.AddTransient<IMessageService, DebugMessageService>();
        }

        /// <summary>
        /// Called during startup. Sets up the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder creating the app.</param>
        /// <param name="env">The environment we're hosting in.</param>
        /// <param name="dbContext">Database context for ASP.NET Identity</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IdentityDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.Migrate();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Not really needed because this is just a REST api.
            //app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "api/{action}",
                    new {
                        controller = "Api",
                    });
            });
        }
    }
}
