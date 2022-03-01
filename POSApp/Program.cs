using Microsoft.Extensions.DependencyInjection;
using POSApp.ConsoleManager;
using POSApp.Log;
using POSApp.POSInterface;
using POSApp.ProductScan;
using POSApp.Repository;

namespace POSApp
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetService<POSScanner>().Run();

        }

        private static void RegisterServices()
        {

            var services = new ServiceCollection();
            services.AddSingleton<IConsoleManager, ConsoleManager.ConsoleManager>();
            services.AddSingleton<ILogger, ConsoleLogger>();
            
         
            services.AddSingleton<IProductRepository, ProductRepository>(sp =>
            {
                var productDataPath = @".\Data\Products.json";
                var logger = sp.GetRequiredService<ILogger>();
                return new ProductRepository(productDataPath, logger);
            });
            services.AddSingleton<IProductScanner, ProductScanner>(sp =>
            {
                var repo = sp.GetRequiredService<IProductRepository>();
                var logger = sp.GetRequiredService<ILogger>();
                return new ProductScanner(repo, logger);    
            });
            services.AddSingleton<POSScanner>();

            _serviceProvider = services.BuildServiceProvider(true);
        }
    }
}
