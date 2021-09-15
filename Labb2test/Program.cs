using Labb2test.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labb2test
{
    class Program
    {
        private static SessionState _sessionState;
        private static MenuState _menuState;
        //public static List<Customer> GetPredefinedUsers()
        //{
        //    return new List<Customer>() {  new Customer("Knatte", "123"),
        //                               new Customer("Fnatte", "321"),
        //                               new Customer("Tjatte", "213") };
        //}
        private static List<Customer> _customers; //initial value
        

        public List<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }




        enum MenuState
        {
            Quit, Welcome, LoggedIn, ShoppingCart
        }
        enum SessionState
        {
            Active, Inactive
        }

        static void Main(string[] args)
        {
            _menuState = MenuState.Welcome;
            _sessionState = SessionState.Active;

            //Session active
            while (_sessionState == SessionState.Active)
            {
                var newCustoomer = GenerateCustomer("Knatte", "123");
                _customers.Add(newCustoomer);
                
                Console.WriteLine(_customers.Count);
                Console.WriteLine(_customers[0].Password);
                Console.ReadLine();
                GenerateCustomer("Knatte", "123");
                Console.WriteLine(_customers.Count);
                Console.ReadLine();
                GenerateCustomer("Knatte", "123");
                Console.WriteLine(_customers.Count);
                Console.ReadLine();
                GenerateCustomer("Knatte", "123");
                Console.WriteLine(_customers.Count);
                Console.ReadLine();


                //Menu1
                while (_menuState == MenuState.Welcome)
                {
                    //RenderWelcomeMenu metod
                    RenderWelcomeMenu();
                }

                while (_menuState == MenuState.LoggedIn)
                {
                    //RenderLoggedinMenu metod
                }
            }
        }

        private static void RenderWelcomeMenu()
        {
            Console.WriteLine("1. Logga in\n2. Registrera användare\n3. Avsluta");
            switch (Console.ReadLine())
            {
                case "1":
                    break;

                case "2":
                    //GenerateCustomer(Console.ReadLine(), Console.ReadLine());
                    
                    break;

                case "3":
                    _sessionState = SessionState.Inactive;
                    _menuState = MenuState.Quit;
                    break;

                default:
                    break;
            }
        }
        public static Customer GenerateCustomer(string newUsername, string newPassword)//string newUsername, string newPassword
        {
            var newCustomer = new Customer(newUsername, newPassword);
            return newCustomer;
        }
    }
}
