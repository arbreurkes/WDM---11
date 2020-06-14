using Grains;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Silo
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Silo : StatelessService
    {
        public Silo(StatelessServiceContext context)
            : base(context)
        { }

        ///// <summary>
        ///// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        ///// </summary>
        ///// <returns>A collection of listeners.</returns>
        //protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        //{
        //    return new ServiceInstanceListener[0];
        //}

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=wdmgroup11;AccountKey=gl81cDAOlt7o/+YoTWUc5tAg3Gn9V0j8JvHoffuR0RCyrPOHsRPSwCTmMuxYBhSrIjIbz/cvc2A28j3CUznVuQ==;EndpointSuffix=core.windows.net";
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                //.UseLocalhostClustering()
                .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "wdm-group11-orleans-silocluster";
                    options.ServiceId = "wdm-group11-orleans-api";
                })
                //Silo-to-silo endpoints, used for communication between silos in the same cluster
                //Client-to-silo endpoints (or gateway), used for communication between clients and silos in the same cluster
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
               //This step will help Orleans to load user assemblies and types.These assemblies are referred
               //to as Application Parts.All Grains, Grain Interfaces, and Serializers are discovered using Application Parts.
               .ConfigureApplicationParts(parts =>
               {
                   parts.AddApplicationPart(typeof(IOrderGrain).Assembly).WithReferences();
                   parts.AddApplicationPart(typeof(OrderGrain).Assembly).WithReferences();
               })
               .UseDashboard(options => { })
               .ConfigureLogging(logging =>
                    logging.AddConsole())
               .AddAzureTableGrainStorage(
                    name: "orderStore",
                    configureOptions: options =>
                    {
                        options.UseJson = true;
                        options.TableName = "orderStore";
                        options.ConnectionString = connectionString;
                    })
               .AddAzureTableGrainStorage(
                    name: "stockStore",
                    configureOptions: options =>
                    {
                        options.UseJson = true;
                        options.TableName = "stockStore";
                        options.ConnectionString = connectionString;
                    })
                .AddAzureTableGrainStorage(
                    name: "userStore",
                    configureOptions: options =>
                    {
                        options.UseJson = true;
                        options.TableName = "userStore";
                        options.ConnectionString = connectionString;
                    });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
