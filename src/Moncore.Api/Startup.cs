using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moncore.Data.Context;
using Moncore.Data.Helpers;
using Moncore.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moncore.Domain.Interfaces.Repositories;

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
            });

            MappingElements.Initialize();
            services.Configure<DbSettings>(config =>
            {
                config.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                config.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<ApplicationContext>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddDebug();
            log.AddConsole();

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
