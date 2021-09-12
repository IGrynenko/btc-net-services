using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using UserService.Helpers;
using UserService.Interfaces;
using UserService.Mappers;
using UserService.Middleware;
using UserService.Models;
using UserService.Services;

namespace UserService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>mc.AddProfile<MappingProfile>());
            var mapper = mappingConfig.CreateMapper();

            services.AddControllers();

            services.AddSingleton(mapper);
            services.AddSingleton<IDataWorker<User>, DataWorker<User>>(_
                => new DataWorker<User>(Configuration.GetSection("MainFolder").Value + "\\" + Configuration.GetSection("DataSource:Users").Value));

            services.AddTransient<IUserService, Services.UserService>();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IDataHostService, DataHostService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataHostService dataHostService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            dataHostService.StartUp();

            app.ValidateOrigin(Configuration.GetSection("ApiGateway").Value);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
