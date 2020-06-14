using Grains;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace API
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton(CreateClusterClient);
            services.AddControllers();
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
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=wdmgroup11;AccountKey=gl81cDAOlt7o/+YoTWUc5tAg3Gn9V0j8JvHoffuR0RCyrPOHsRPSwCTmMuxYBhSrIjIbz/cvc2A28j3CUznVuQ==;EndpointSuffix=core.windows.net";
            IClusterClient client;
            client = new ClientBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "wdm-group11-orleans-silocluster";
                    options.ServiceId = "wdm-group11-orleans-api";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .UseAzureStorageClustering(options =>
                    options.ConnectionString = connectionString)
                .ConfigureApplicationParts(p =>
                {
                    p.AddApplicationPart(typeof(IOrderGrain).Assembly);
                    p.AddApplicationPart(typeof(OrderGrain).Assembly);
                })
                .Build();

            client.Connect().Wait(); //Catch exception if it does not connects
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }
    }
}
