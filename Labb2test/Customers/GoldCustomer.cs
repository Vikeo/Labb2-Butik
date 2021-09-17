using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2test.Customers
{
    class GoldCustomer : Customer
    {
        public GoldCustomer(string username, string password, Membership membership) : base(username, password)
        {
            this.Username = username;
            this.Password = password;
            this.Membership = membership;
        }

        public override double CalculateSumBasedOnMembership(Membership membership, double sumInSEK)
        {
            sumInSEK = sumInSEK * 0.85;
            return sumInSEK;
        }
    }
}
