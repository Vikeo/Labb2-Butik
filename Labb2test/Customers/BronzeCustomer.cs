﻿using System;

namespace Labb2test.Customers
{
    class BronzeCustomer : Customer
    {
        private string _password;
        private string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public BronzeCustomer(string username, string password, Membership membership) : base(username, password)
        {
            this.Username = username;
            this.Password = password;
            this.Membership = membership;
        }

        public override double CalculateSumBasedOnMembership(double sumInSEK)
        {
            return sumInSEK * 0.95;
        }

        public override string ToString()
        {
            return String.Format($"{Username}鯨{Password}鯨{Membership}鯨");
        }
    }
}
