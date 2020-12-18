using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CW.BlazorDemo.Client.CQRS;
using CW.BlazorDemo.Client.Models;
using CW.Contracts.CQRS;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CW.BlazorDemo.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var container = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = true });
            container.RegisterFrom<CompositionRoot>();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.ConfigureContainer(new LightInjectServiceProviderFactory(container));
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient<IQueryExecutor, QueryExecutor>();
            builder.Services.AddHttpClient<ICommandExecutor, CommandExecutor>();
            builder.Services.AddSingleton<ISession>(new Session());


            await builder.Build().RunAsync();
        }
    }
}
