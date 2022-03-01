using POSApp.Log;
using POSApp.Models;
using POSApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POSApp.ProductScan
{
    public class ProductScanner : IProductScanner
    {

        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;
        private List<Product> _products;
      

        public ProductScanner(IProductRepository productRepository, ILogger logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        private float CalculateTotalCost(IEnumerable<Product> products)
        {
            foreach (var item in products)
            {
                int single;
                int combo;
                if (item.ComboQuantity != 0)
                {
                    combo = Math.DivRem(item.Quantity, item.ComboQuantity, out single);

                }
                else
                {
                    single = item.Quantity;
                    combo = 0;
                }

                var total = (single * item.SinglePrice) + (combo * item.ComboPrice);
                item.Total = total;
            }

            return products.Sum(item => item.Total);
        }


        public float ScanProductAndCalculateCost(string products)
        {
            var scannedProducts = new List<Product>();

            try
            {
                if (String.IsNullOrEmpty(products))
                {
                    throw new ArgumentNullException(nameof(products));
                }
                _products = _productRepository.Products;
                var charProduct = products.ToUpper().ToCharArray();
                foreach (var item in charProduct)
                {
                    var product = _products.Find(x => char.Parse(x.ProductName.ToUpper()).Equals(item));

                    if (product == null)
                    {
                        _logger.LogMessage($"This product: {item} is not present in product database");
                        return -1; 
                    }
                    if (product != null)
                    {
                        if (!scannedProducts.Any(x => x.ProductName == product.ProductName))
                        {
                            scannedProducts.Add(product);

                        }
                        else
                        {
                            var updatedProdcut = scannedProducts.FirstOrDefault(x => x.ProductName == product.ProductName);
                            updatedProdcut.Quantity++;
                        }

                    }
                }
                return CalculateTotalCost(scannedProducts);
            }
            catch (Exception ex)
            { 
                _logger.LogMessage($"Exception: {ex.Message}");
                return 0;
            }
            
        }
    }
}

