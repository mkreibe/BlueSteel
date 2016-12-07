using System.IO;
using BlueSteel.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BlueSteel.Host
{
    /// <summary>
    /// Defines the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">The environment variables.</param>
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var management = new WebHostBuilder()
                .UseUrls("http://localhost:5001/")
                .UseKestrel()
                .UseIISIntegration()
                .BuildManagement<ActuatorStartup>();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseManagementHost(management)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
