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
        //Implementation för totalpris

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
        public double CartSum
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
            this.CartSum = 0;
        }

        public override string ToString()
        {
            string jointString = string.Join(",", Cart);
            return String.Format($"{Username}, {Password}, {jointString}");
            //Ska skriva ut namn, lösenord och kundvagn
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

        //Skriva med om det är bronze, silver eller guld här eller i de egna klasserna?
        public virtual double CalculateSumBasedOnMembership(double sumInSEK)
        {
            return sumInSEK;
        }
    }
}
