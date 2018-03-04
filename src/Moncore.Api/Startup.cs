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
using Moncore.Api.Services;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;
using Moncore.Domain.Validations;
using Moncore.Domain.Helpers;
using Moncore.CrossCutting.Interfaces;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Interfaces.Services;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

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
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddFluentValidation();

            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("v1", new Info {Title = "Moncore", Version = "v1", Contact = new Contact{Name = "Guilherme", Email = "guisfits@hotmail.com", Url = "github.com/guisfits/Moncore"}, Description = "RESTful API using ASP.NET Core and MongoDB"});
            });

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
            services.AddScoped<PaginationParameters<User>, UserParameters>();

            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<Post>, PostValidator>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(config =>
            {
                var actionContext = config.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddTransient<IEntityHelperServices, EntityHelperService>();

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
