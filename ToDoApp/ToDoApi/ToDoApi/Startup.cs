using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using ToDo.Infrastructure;
using ToDoApi.Authorization;
using ToDoApi.Options;
using ToDoApi.Services;

namespace ToDoApi
{
    public class Startup
    {
        private ILogger _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "To Do Api", Version = "v1" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<ToDoListsService>();

            services.AddDbContext<ToDoDbContext>
                (options => options.UseSqlServer(Configuration["ConnectionString"]));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.Configure<ReminderServiceOptions>(Configuration.GetSection("ReminderServiceOptions"));
            services.AddHostedService<ReminderService>();
            services.AddHostedService<ShareExpiredService>();

            string domain = $"https://{Configuration["Auth0:Domain"]}/";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:to-do-list", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("read:to-do-list", domain)));

                options.AddPolicy("write:to-do-list", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("write:to-do-list", domain)));

                options.AddPolicy("delete:to-do-list", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("delete:to-do-list", domain)));

                options.AddPolicy("read:to-do-item", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("read:to-do-item", domain)));

                options.AddPolicy("write:to-do-item", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("write:to-do-item", domain)));

                options.AddPolicy("delete:to-do-item", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("delete:to-do-item", domain)));

                options.AddPolicy("write:to-do-list-share", policy => policy
                .Requirements
                .Add(new HasScopeRequirement("write:to-do-list-share", domain)));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddTransient(s =>
                s.GetService<IHttpContextAccessor>().HttpContext.User);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
            _logger = loggerFactory.CreateLogger("Shutdown logger");

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "To Do Api");
            });

            app.UseCors("MyPolicy");

            UpdateDatabase(app);
        }

        private void OnShutdown()
        {
            _logger.LogInformation("ToDoApi ended!");
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ToDoDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
