using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Components.Consumers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Service
{
    //The cool thing about dotnet 3.1 is that they actually have the new generic host
    //and you can create a console application that will install as service, it will run as 
    //a Daemon on Linux and it has all of the microsoft extension stuff built in. So configuration
    //command line, I mean environment variables, everything is all built in, logging, etc.

    //We took our message consumer out of process and put it in a separate service (this one) 
    class Program
    {
        static async Task Main(string[] args)
        {
            //From https://github.com/MassTransit/Sample-ConsoleService
                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                var builder = new HostBuilder()
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.json", optional: true);
                        config.AddEnvironmentVariables();

                        if (args != null)
                            config.AddCommandLine(args);
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                        services.AddMassTransit(cfg =>
                        {
                            cfg.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();
                            cfg.AddBus(ConfigureBus);
                            //cfg.AddRequestClient<IsItTime>();
                        });

                        services.AddHostedService<MassTransitConsoleHostedService>();
                        //services.AddHostedService<CheckTheTimeService>();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    });

                if (isService)
                {
                    await builder.UseWindowsService().Build().RunAsync();
                }
                else
                {
                    await builder.RunConsoleAsync();
                }
            }

            static IBusControl ConfigureBus(IServiceProvider provider)
            {
                return Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.ConfigureEndpoints(provider);
                });
            }

       
    }
}
