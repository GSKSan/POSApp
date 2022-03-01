using POSApp.Models;
using System.Collections.Generic;

namespace POSApp.ProductScan
{
    public interface IProductScanner
    {
        float ScanProductAndCalculateCost(string products);
    }


}

