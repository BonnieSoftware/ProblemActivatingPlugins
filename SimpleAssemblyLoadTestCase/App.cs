using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleAssemblyLoadTestCase.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SimpleAssemblyLoadTestCase
{
    public class App
    {
        private static void Main()
        {
            var loggerFactory = LoggerFactory.Create(
                builder => builder.AddConsole()
            );
            
            var services = new ServiceCollection()
                .AddSingleton(loggerFactory)
                .BuildServiceProvider();

            string pluginPath = Path.GetFullPath(@"..\..\..\..\ExamplePlugin\bin\Debug\netcoreapp3.0\ExamplePlugin.dll");
            var loadContext = new PluginLoadContext(pluginPath, new[] {
                typeof(ILogger).Assembly.GetName().Name,
                typeof(IPlugin).Assembly.GetName().Name
            });

            Assembly assembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
            IEnumerable<IPlugin> plugins = CreatePluginInstances(services, assembly);

            foreach (IPlugin plugin in plugins)
            {
                plugin.Initialise();
            }
        }

        protected static IEnumerable<IPlugin> CreatePluginInstances(ServiceProvider services, Assembly pluginAssembly)
        {
            foreach (Type type in pluginAssembly.GetTypes())
            {
                yield return ActivatorUtilities.CreateInstance(services, type) as IPlugin;
            }
        }
    }
}
