using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2test.Customers
{
    class BronzeCustomer : Customer
    {
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
    }
}
