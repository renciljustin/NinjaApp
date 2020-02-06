using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NinjaApp.Core;
using NinjaApp.Core.Models;
using NinjaApp.Core.Seeds;
using NinjaApp.Data;
using NinjaApp.Persistence;
using NinjaApp.Shared.Enums;

namespace NinjaApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddCors();

            services.AddSingleton<WeatherForecastService>();

            services.AddDbContext<NinjaDbContext>
                (opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(Startup));
            
            var builder = services.AddIdentityCore<User>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequiredLength = 8;
            });
            
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<NinjaDbContext>()
                .AddDefaultTokenProviders();

             services.AddAuthentication(opt => {
                 opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Token:Key"])),
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(opt => {
                opt.AddPolicy(PolicyPrefix.RequiresAdmin, p => p.RequireRole(RolePrefix.Admin));
                opt.AddPolicy(PolicyPrefix.RequiresModerator, p => p.RequireRole(RolePrefix.Moderator));
                opt.AddPolicy(PolicyPrefix.RequiresUser, p => p.RequireRole(RolePrefix.User, RolePrefix.User));
            });
            
            services.AddTransient<SeedUsersAndRoles>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedUsersAndRoles seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            seed.BeginSeeding();

            app.UseAuthentication();
            app.UseCors(opt =>
                opt.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
