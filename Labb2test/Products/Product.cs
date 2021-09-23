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

        private int _productQuanity;
        public int ProductQuantity
        {
            get { return _productQuanity; }
            set { _productQuanity = value; }
        }

        public Product(string productName, double productPrice)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            ProductQuantity = 1;

        }

        public static List<Product> GenerateListOfProducts()
        {
            return new List<Product>() { new Product("Penna", 5),
                                         new Product("Annanas", 20),
                                         new Product("Äpple", 4),
                                         new Product("Penna (lång)", 30)};
        }

        //ToString som skrivers ut när man bara vill skriva ut en produkt.
        public string ToString()
        {
            return $"\t{ProductQuantity}st {ProductName,-12}{ProductPrice * ProductQuantity,13:C} (styckpris: {ProductPrice} kr)\n";
        }

        //ToString för när man vill kunna ändra valutan som skrivs ut på¨produkten.
        public string ToString(string userInput)
        {
            switch (userInput)
            {
                case "SEK":
                    return $"\t{ProductName,-12:C}{ProductPrice.ToString("C", CultureInfo.CreateSpecificCulture("sv-SE")),13}\n";

                case "EUR":
                    return $"\t{ProductName,-12:C}{ConvertSumPriceInEUR(ProductPrice).ToString("C", CultureInfo.CreateSpecificCulture("fr-FR")),13}\n";

                case "JPY":
                    return $"\t{ProductName,-20:C}{ConvertSumPriceInJPY(ProductPrice).ToString("C", CultureInfo.CreateSpecificCulture("ja-JP")),-10}\n";

                default:
                    return $"\t{ProductName,-12:C}{ProductPrice,10:C}\n";
            }
        }

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
