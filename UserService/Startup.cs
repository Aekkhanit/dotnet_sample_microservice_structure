using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Handles;
using UserService.Helpers;
using UserService.Infrastructure;
using UserService.Infrastructure.GenericRepository;
using UserService.Infrastructure.SpecificRepository;
using UserService.Services;

namespace UserService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public IConfiguration _Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());

            services.AddCustomMvc(_Configuration)
                          .AddCustomAuthentication(_Configuration)
                          .AddApplicationServices(_Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var pathBase = _Configuration.GetValue<string>("APP_CONFIG:Base_Path");
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var culture = _Configuration["APP_CONFIG:Culture"];
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture),
                SupportedCultures = new[] { CultureInfo.GetCultureInfo(culture) },
                SupportedUICultures = new[] { CultureInfo.GetCultureInfo(culture) }
            });

            _Configuration.GetSection(nameof(SwaggerOptions)).Bind(new SwaggerOptions());

            app.UseSwagger(c =>
            {
                c.RouteTemplate = !string.IsNullOrEmpty(pathBase) ? pathBase : ".." + "/swagger/{documentname}/swagger.json";
            });

            app.UseSwagger().UseSwaggerUI(c =>
            {

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : "..") }/swagger/{description.GroupName}/swagger.json", $"{_Configuration["APP_CONFIG:NAME"]} API {description.GroupName.ToUpperInvariant()}");
                }
                c.OAuthClientId(string.Empty);
                c.OAuthClientSecret(string.Empty);
                c.OAuthRealm(string.Empty);
                c.OAuthAppName($"{_Configuration["APP_CONFIG:NAME"]} API Swagger UI");
            });


            app.UseMiddleware<ErrorMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();


            app.UseRouting();
            app.UseCors("AppPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }


    }

    public static class ServiceCollectionExtensions
    {
        public static readonly string[] total_version = { "1", "2" };
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            //var identityUrl = configuration.GetValue<string>("urls:identity");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;
            });

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration _Configuration)
        {
            services.AddOptions();
            services.AddMemoryCache();
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                foreach (var version in total_version)
                {
                    options.SwaggerDoc("v" + version, new OpenApiInfo
                    {
                        Title = $"{_Configuration["APP_CONFIG:NAME"]} API for Web Clients",
                        Version = "v" + version,
                        Description = $"{_Configuration["APP_CONFIG:NAME"]} API for Web Clients"
                    });

                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,

                            },
                            new List<string>()
                        }
                    });
            });

            services.AddApiVersioning(x =>
            {
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning();

            services.AddCors(options =>
            {
                options.AddPolicy("AppPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddControllersWithViews();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Core Dependency
            services.AddHttpContextAccessor();

            //Add Context & Generic
            services.AddDbContext<MyDB_Context>(options => options.UseSqlServer(configuration["APP_CONFIG:ConnectionString"]));

            //Add Scoped 
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<UserService.Module.v1.IUserModule, UserService.Module.v1.UserModule>();
            services.AddScoped<UserService.Module.v2.IUserModule, UserService.Module.v2.UserModule>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAuthorizeRepository, UserAuthorizeRepository>();

            //Add Transient
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IInternalService, InternalService>();
            services.AddTransient<IVendorService, VendorService>();
            services.AddTransient<IWebhookService, WebhookService>();

            //Add Singleton
            services.AddSingleton<IJwtHelper, JwtHelper>();

            return services;
        }


    }
}
