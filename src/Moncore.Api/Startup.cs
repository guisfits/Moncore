using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moncore.Data.Context;
using Moncore.Data.Helpers;
using Moncore.Data.Repositories;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Moncore.Api.Interfaces;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;
using Moncore.Domain.Validations;

namespace Moncore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            })
            .AddFluentValidation();

            MappingElements.Initialize();
            services.Configure<DbSettings>(config =>
            {
                config.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                config.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            #region IoC

            services.AddScoped<ApplicationContext>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Post>, PostValidator>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(config =>
            {
                var actionContext = config.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, IPropertyMappingService>();

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddConsole();
            log.AddDebug(LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => 
                {
                    appBuilder.Run(async handler => 
                    {
                        handler.Response.StatusCode = 500;
                        await handler.Response.WriteAsync("An Error occurred");
                    });
                });
            }

            app.UseMvc();
        }
    }
}
