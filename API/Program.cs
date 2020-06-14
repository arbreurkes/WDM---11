using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.Configuration;
using System.Threading.Tasks;
using Grains;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace ShoppingCart
{
    public class Program
    {
        public static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.Configure((ctx, app) =>
                    {
                        if (ctx.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                        }
                        else
                        {
                            app.UseExceptionHandler(errorApp =>
                            {
                                errorApp.Run(async context =>
                                {
                                    
                                    context.Response.StatusCode = 500; // or 404 ? 
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync("Forbidden");
                                    await context.Response.CompleteAsync();

                                });
                            });
                        }

                        app.UseHttpsRedirection();
                        app.UseRouting();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                        app.UseOrleansDashboard();
                        app.Map("/dashboard", d =>
                        {
                            d.UseOrleansDashboard();
                        });
                     

                    });
                })
                .UseOrleans(siloBuilder =>
                {
                    IConfiguration conf = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
                    string connectionString = conf.GetConnectionString("BartsAzureTableStorage");
                    siloBuilder
                    .UseLocalhostClustering()
                    //.UseAzureStorageClustering(options => options.ConnectionString = connectionString)
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
                .Configure<GrainCollectionOptions>(options =>
                {
                    options.CollectionAge = TimeSpan.FromMinutes(5);

                })
                .AddAzureBlobGrainStorage(
                    name: "orderStore",
                    configureOptions: options =>
                    {
                        options.UseJson = true;
                        //options.TableName = "orderStore";
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
                .UseDashboard(opts => {
                    opts.HostSelf = false;
                })
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000)
                .AddAzureTableTransactionalStateStorage(
                    name: "transactionStore",
                    configureOptions: options =>
                    {
                        options.TableName = "transactionStore";
                        options.ConnectionString = connectionString;
                    })
                .UseTransactions();
                })
                .ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();
                })
            .Build()
            .StartAsync();
    }
}
