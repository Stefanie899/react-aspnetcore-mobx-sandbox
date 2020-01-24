using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Sandbox.Presentation.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseLibuv(opts => opts.ThreadCount = 4)
                .UseKestrel()
                .ConfigureKestrel((context, options) => { })
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}
