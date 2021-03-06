using Labb2test.Products;
using System;
using System.Collections.Generic;

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

        public string _password;
        public string Password
        {
            get { return _password; }
            private set { _password = value; }
        }

        private List<Product> _cart;
        public List<Product> Cart { get { return _cart; } }

        private double _cartSum;
        public double CartSumInSEK
        {
            get { return _cartSum; }
            set { _cartSum = value; }
        }

        private int _cartQuanity;
        public int CartQuantity
        {
            get { return _cartQuanity; }
            set { _cartQuanity = value; }
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
            CartQuantity = 0;
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

        //Jämför ett input lösenord med en kunds lösenord.
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
