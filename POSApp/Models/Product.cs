namespace POSApp.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public float SinglePrice { get; set; }
        public int ComboQuantity { get; set; }
        public float ComboPrice { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }

        public Product(string productName)
        {
            ProductName = productName;
            Quantity = 1;
            Total = 0;
        }
    }
}

