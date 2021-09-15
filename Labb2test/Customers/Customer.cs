using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2test.Customers
{
    class Customer
    {
        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public Customer(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
        public override string ToString()
        {
            return String.Format($"{Username}鯨{Password}鯨");
        }
    }
}
