using POSApp.ConsoleManager;
using POSApp.ProductScan;
using POSApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSApp.POSInterface
{
    public class POSScanner 
    {
        private readonly IConsoleManager _consoleManager;
        private readonly IProductScanner _productScanner;
        private readonly IProductRepository _productRepository;

        public POSScanner(IConsoleManager consoleManager, IProductScanner productScanner, IProductRepository productRepository)
        {
            _consoleManager = consoleManager ?? throw new ArgumentNullException(nameof(consoleManager));
            _productScanner = productScanner ?? throw new ArgumentNullException(nameof(productScanner));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }
        public void Run()
        {
         
            
            _productRepository.ReadProducts();
            var isAllScanned = false;
            while (!isAllScanned)
            {
                _consoleManager.WriteLine("Scan Products and press enter when finsihed");

                var products = _consoleManager.ReadLine();

                var totalCost = _productScanner.ScanProductAndCalculateCost(products);
                if(totalCost == -1)
                {
                    _consoleManager.WriteLine("Invalid products are scanned, please scan valid products");
                    break;
                }
              
           
                _consoleManager.WriteLine($"Your total bill amount is ${totalCost}");

                _consoleManager.WriteLine("Do you want to continue? Type '#' to exit");

                var continueScanning = _consoleManager.ReadLine();
                if (continueScanning.ToLower().Equals("#"))
                {
                    isAllScanned = true;
                    _consoleManager.WriteLine("Thanks for using the POSScanner");

                }
                else
                {
                    continue;
                }

            }
        }
    }
}
