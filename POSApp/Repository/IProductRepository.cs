using POSApp.Models;
using System.Collections.Generic;

namespace POSApp.Repository
{
    public interface IProductRepository
    {
        List<Product> Products { get; }
        void ReadProducts();
    }
}

