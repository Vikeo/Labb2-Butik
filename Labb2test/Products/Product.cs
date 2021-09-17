using System.Collections.Generic;

namespace Labb2test.Products
{
    class Product
    {
        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        private double _productPrice;
        public double ProductPrice
        {
            get { return _productPrice; }
            set { _productPrice = value; }
        }
        public Product(string productName, double productPrice)
        {
            ProductName = productName;
            ProductPrice = productPrice;
        }
        public static List<Product> GenerateListOfProducts()
        {
            return new List<Product>() { new Product("Penna", 5),
                                         new Product("Annanas", 20),
                                         new Product("Äpple", 4),
                                         new Product("Penna (lång)", 30)};
        }
        public override string ToString()
        {
            return $"{ProductName}\t{ProductPrice}:-";
        }
    }
}
