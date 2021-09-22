using System;
using System.Collections.Generic;
using Labb2test.Products;

namespace Labb2test.Customers
{
    enum Membership
    {
        NonMember, Bronze, Silver, Gold
    }
    abstract class Customer
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        private string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private List<Product> _cart;
        public List<Product> Cart { get { return _cart; } }

        private double _cartSum;
        public double CartSumInSEK
        {
            get { return _cartSum; }
            set { _cartSum = value; }
        }

        private Membership _membership;

        public Membership Membership
        {
            get { return _membership; }
            set { _membership = value; }
        }
        public Customer(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this._cart = new List<Product>();
            this.CartSumInSEK = 0;
        }

        public override string ToString()
        {
            string jointString = "";

            foreach (var product in Cart)
            {
                if (product.ProductQuantity != 0)
                {
                    jointString += product.ToString();
                }
            }

            return String.Format($"Användarnamn: {Username}" +
                $"\nLösenord: {Password}" +
                $"\nKundvagnen innehåller: \n{jointString}");
        }

        public static bool VerifyPassword(string password, Customer customer)
        {
            if (password == customer.Password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetPassword(Customer customer)
        {
            return customer.Password;
        }

        public virtual double CalculateSumBasedOnMembership(double sumInSEK)
        {
            return sumInSEK;
        }
    }
}
