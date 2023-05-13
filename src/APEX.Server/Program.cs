
using APEX.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APEX.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);
            var app = Configure(builder);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            var connString = configuration.GetConnectionString("AzureSQL"); //TODO: move to secret
            services.AddDbContext<ApexDbContext>(options => options.UseSqlServer(connString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApexUser, IdentityRole<long>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            }).AddEntityFrameworkStores<ApexDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        private static WebApplication Configure(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}