using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Interfaces;
using TechNerd.Azure.Cosmos.Table.StorageHelper.Core;
using TechNerd.Azure.Cosmos.Table.StorageHelper.DTO;

namespace TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer
{
    public class Startup
    {
        private StorageConfig _storageConfig = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _storageConfig = configuration.GetSection(nameof(StorageConfig)).Get<StorageConfig>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITableStorage<string, UserEntity>>(op =>
            {
                return new TableStorage<string, UserEntity>("Users", new StorageDBContext(_storageConfig));
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechNerd.Azure.Cosmos.Table.StorageHelper.Consumer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
