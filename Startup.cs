using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lidl_twitter_user_service.Data;
using lidl_twitter_user_service.SyncDataServices.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.Logging;

namespace lidl_twitter_user_service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ITweetDataClient, HttpTweetDataClient>();

            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using MySQL server Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseMySQL(Configuration.GetConnectionString("UserServiceDB")));
            }
            else
            {
                Console.WriteLine("--> Using InMemory Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMemory"));
            }

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers();
            
            Console.WriteLine($"--> TweetService Endpoint: {Configuration["TweetService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

          PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
