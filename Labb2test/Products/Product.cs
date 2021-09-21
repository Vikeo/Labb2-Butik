using System;
using System.Collections.Generic;
using System.Globalization;

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
  
        public string ToString()
        {
            return $"{ProductName,-15:C}{ProductPrice,10:C}\n";
        }
        public string ToString(string userInput)
        {
            switch (userInput)
            {
                case "SEK":
                    return $"{ProductName,-15:C}{ProductPrice.ToString("C", CultureInfo.CreateSpecificCulture("sv-SE")),10}\n";

                case "EUR":
                    return $"{ProductName,-15:C}{ConvertSumPriceInJPY(ProductPrice).ToString("C", CultureInfo.CreateSpecificCulture("fr-FR")),10}\n";

                case "JPY":
                    return $"{ProductName,-15:C}{ConvertSumPriceInJPY(ProductPrice).ToString("C", CultureInfo.CreateSpecificCulture("ja-JP")),10}\n";

                default:
                    return $"{ProductName,-15:C}{ProductPrice,10:C}\n";
            }
        }

        //DRY??????
        public static double ConvertSumPriceInEUR(double priceInSEK)
        {
            var conversionRate = 0.0984775f;
            var sumInEURO = priceInSEK * conversionRate;
            return Math.Round(sumInEURO, 2);
        }

        public static double ConvertSumPriceInJPY(double priceInSEK)
        {
            var conversionRate = 12.7529f;
            var sumInYEN = priceInSEK * conversionRate;
            return Math.Round(sumInYEN, 2);
        }
    }
}
