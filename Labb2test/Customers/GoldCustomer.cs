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

        public override double CalculateSumBasedOnMembership(double sumInSEK)
        {
            return sumInSEK * 0.85;
        }
    }
}
