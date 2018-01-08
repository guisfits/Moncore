using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moncore.Data.Context;
using Moncore.Data.Helpers;
using Moncore.Data.Repositories;
using Moncore.Domain.Interfaces;

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
            services.AddMvc();

            MappingElements.Initialize();
            services.Configure<DbSettings>(config =>
            {
                config.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                config.Database = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<ApplicationContext>();
            services.AddTransient<IAlbumRepository, AlbumRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IToDoRepository, ToDoRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "Default",
                    template: "api/{controller}/{id?}",
                    defaults: new {controller = "User", action = "Get"});
            });
        }
    }
}
