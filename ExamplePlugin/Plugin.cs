using Microsoft.Extensions.Logging;
using SimpleAssemblyLoadTestCase.Interfaces;

namespace ExamplePlugin
{
    public class Plugin : IPlugin
    {
        private readonly ILogger _logger;

        public string Name => "Example";

        public string Description => "Example plugin";

        // Pull in dependencies like so...
        public Plugin(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Plugin>();
        }

        public void Initialise()
        {
            _logger?.LogInformation("Initialising plugin.");

            //var window = new MainWindow();
            //window.Show();
        }
    }
}
