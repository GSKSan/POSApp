
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POSApp.Models;
using POSApp.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POSApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        private string _productsFile;
        private List<Product> _products;
        private readonly ILogger _logger;    


        public ProductRepository(string productsFile,ILogger logger)
        {
            _productsFile = productsFile ?? throw new ArgumentNullException(nameof(productsFile));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Product> Products
        {
            get => _products;
        }

        public void ReadProducts()
        {
            try
            {
                using (StreamReader file = File.OpenText(_productsFile))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    var jProducts = (JArray)JToken.ReadFrom(reader);
                    _products = jProducts.ToObject<List<Product>>();
                    _logger.LogMessage("Products loaded from JSON file");
                }
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Exception: {ex.Message}");
            }
        }
    }
}

