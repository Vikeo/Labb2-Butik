using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private long _productPrice;
        public long ProductPrice
        {
            get { return _productPrice; }
            set { _productPrice = value; }
        }
        public Product(string productName, long productPrice)
        {
            ProductName = productName;
            ProductPrice = productPrice;
        }

        public static List<Product> GenerateListOfProducts()
        {
            return new List<Product>() { new Product("Penna", 24), 
                                         new Product("Annanas", 24), 
                                         new Product("Äpple", 24),
                                         new Product("Penna (lång)", 24)};
        }
        public override string ToString()
        {
            return $"{ProductName}\t{ProductPrice}:-";
        }
    }
}
