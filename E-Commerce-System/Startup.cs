using E_Commerce_System.Common;
using E_Commerce_System.Models;
using E_Commerce_System.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System
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
            services.Configure<MongoDBSettings>(
                Configuration.GetSection(nameof(MongoDBSettings)));

            services.AddSingleton<IMongoDBSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

            services.Configure<Neo4jSettings>(
                Configuration.GetSection(nameof(Neo4jSettings)));

            services.AddSingleton<INeo4jSettings>(sp =>
                sp.GetRequiredService<IOptions<Neo4jSettings>>().Value);

            services.Configure<SqlServerSettings>(
                Configuration.GetSection(nameof(SqlServerSettings)));

            services.AddSingleton<ISqlServerSettings>(sp =>
                sp.GetRequiredService<IOptions<SqlServerSettings>>().Value);

            services.AddDbContext<ADBMSContext>(options =>
                options.EnableSensitiveDataLogging(true)
                        .UseSqlServer(Configuration.GetConnectionString("ADBMSConnection")));

            JWTSetting.Key = Configuration["Jwt:Key"];
            JWTSetting.Issuer = Configuration["Jwt:Issuer"];
            JWTSetting.Audience = Configuration["Jwt:Audience"];
            JWTSetting.Subject = Configuration["Jwt:Subject"];


            services.AddControllers();
            services.AddSingleton<BookService>();
            services.AddSingleton<Neo4jService>();
            services.AddScoped<INeo4jService, Neo4jService>();
            services.AddScoped<IBookNeo4jService, BookNeo4jService>();
            services.AddScoped<IUserService, UserService>();
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddMvc();
            services.AddSession();
            //services.AddMvc();

            //services.AddSession(options =>
            //{
            //    options.Cookie.Name = "JWToken";
            //    options.IdleTimeout = TimeSpan.FromHours(5);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});

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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
            
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

        }
    }
}
