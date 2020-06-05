using Infrastructure.Interfaces;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.TestingHost;
using OrleansBasics;
using System.Net;

namespace Test.OrleansTest
{

    public class TestSiloConfigurations : ISiloConfigurator
    {
     
    public void Configure(ISiloBuilder siloBuilder)
            {

            string connectionString = "DefaultEndpointsProtocol=https;AccountName=wdmgroup11;AccountKey=gl81cDAOlt7o/+YoTWUc5tAg3Gn9V0j8JvHoffuR0RCyrPOHsRPSwCTmMuxYBhSrIjIbz/cvc2A28j3CUznVuQ==;EndpointSuffix=core.windows.net";
            siloBuilder
            .UseLocalhostClustering()
            
            .Configure<ClusterOptions>(opts =>
            {
                opts.ClusterId = "wdm-group11-orleans-silocluster";
                opts.ServiceId = "wdm-group11-orleans-api";
            })
        .ConfigureApplicationParts(parts =>
        {
            parts.AddApplicationPart(typeof(IOrderGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(OrderGrain).Assembly).WithReferences();
        })
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
            })
        .Configure<EndpointOptions>(opts =>
        {
            opts.AdvertisedIPAddress = IPAddress.Loopback;
        });
   
            }

    }

  
}

