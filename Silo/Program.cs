using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleansBasics
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
               .ConfigureApplicationParts(parts => parts.AddFromApplicationBaseDirectory())
               .UseDashboard(options => { })
               .ConfigureLogging(logging =>
                    logging.AddConsole()).AddAzureTableGrainStorage(
                    name: "wdmgroup11",
                    configureOptions: options =>
                    {
                        options.UseJson = true;
                        options.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=wdmgroup11;AccountKey=gl81cDAOlt7o/+YoTWUc5tAg3Gn9V0j8JvHoffuR0RCyrPOHsRPSwCTmMuxYBhSrIjIbz/cvc2A28j3CUznVuQ==;EndpointSuffix=core.windows.net";
                    });

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}