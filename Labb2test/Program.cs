using Labb2test.Customers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Labb2test
{
    class Program
    {
        private static SessionState _sessionState;
        private static MenuState _menuState;
        private static List<Customer> _customers = new List<Customer>();
        private static string _loggedInCustomer = "";
        private static readonly string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        enum MenuState
        {
            Quit, Welcome, LoggedIn, ShoppingCart
        }
        enum SessionState
        {
            Active, Terminate
        }

        static void Main(string[] args)
        {
            FetchSavedCustomers();

            _menuState = MenuState.Welcome;
            _sessionState = SessionState.Active;

            //Session active
            while (_sessionState == SessionState.Active)
            {
                Console.Clear();
                //Menu1
                while (_menuState == MenuState.Welcome)
                {
                    //RenderWelcomeMenu metod
                    RenderWelcomeMenu();
                }

                while (_menuState == MenuState.LoggedIn)
                {
                    RenderLoggedinMenu();
                }
            }
        }

        private static void FetchSavedCustomers()
        {
            using (var sr = new StreamReader(Path.Combine(_docPath, "Customers.txt")))
            {
                var text = sr.ReadToEnd();
                Console.WriteLine(text);
                string[] splitText = text.Split('鯨');
                for (int i = 0; i < splitText.Length - 1; i += 2)
                {
                    string savedUsername = splitText[i];
                    string savedPassword = splitText[i + 1];
                    var savedCustomer = new Customer(savedUsername, savedPassword);
                    _customers.Add(savedCustomer);
                }
                foreach (var customer in _customers)
                {
                    Console.WriteLine(customer.Username + " " + customer.Password);
                }
            }
        }

        private static void RenderLoggedinMenu()
        {
            Console.Clear();
            Console.WriteLine($"Du är inloggad som \"{_loggedInCustomer}\"\n--------------------------------------------------------\n1. Handla\n2. Se kundvagnen\n3. Gå till kassan\n4. Logga ut");
            switch (Console.ReadLine())
            {
                case "1":
                    
                    break;

                case "2":
                    
                    break;

                case "3":
                    
                    break;

                case "4":
                    LogoutCustomer();
                    break;

                default:
                    break;
            }
        }

        private static void LogoutCustomer()
        {
            Console.Clear();
            _loggedInCustomer = "";
            _menuState = MenuState.Welcome;
            Console.WriteLine("Du loggades ut");
            Console.ReadLine();
        }

        private static void RenderWelcomeMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Logga in\n2. Registrera dig som ny kund\n3. Avsluta");
           
            Console.WriteLine($"\nAntal registrerade kunder: {_customers.Count}");

            switch (Console.ReadLine())
            {
                case "1":
                    LoginCustomer();
                    break;

                case "2":
                    
                    GenerateCustomer();
                    break;

                case "3":
                    QuitApplication();
                    break;

                default:
                    break;
            }
        }

        private static void LoginCustomer()
        {
            bool tryAgain = true;
            while (tryAgain)
            {
            Console.Clear();
            Console.WriteLine("Logga in");
            Console.Write("Skriv in ditt användarnamn: ");
            string loginUsername = Console.ReadLine();
            
                foreach (var customer in _customers)
                {
                    if (loginUsername == customer.Username)
                    {
                        Console.Write("Skriv in ditt lösenord: ");
                        string loginPassword = Console.ReadLine();
                        if (loginPassword == customer.Password)
                        {
                            Console.Write($"\nDu är nu inloggad som \"{customer.Username}\"!!");
                            Console.ReadLine();
                            _menuState = MenuState.LoggedIn;
                            _loggedInCustomer = customer.Username;
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Lösenordet är fel. Försök igen?\n1. Försök igen\n2. Gå tillbaka till huvudmenyn.");
                            bool continueLoop2 = true;
                            while (continueLoop2)
                            {
                                switch (Console.ReadLine())
                                {
                                    case "1":
                                        continueLoop2 = false;
                                        break;

                                    case "2":
                                        tryAgain = false;
                                        continueLoop2 = false;
                                        break;

                                    default:
                                        break;
                                }
                            }
                            return;
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("Användarnamnet finns inte i våran databas, vill du registrera dig som ny kund eller försöka igen?");
                Console.WriteLine("1. Registrera dig som kund\n2. Försök igen");
                bool continueLoop = true;
                while (continueLoop)
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            GenerateCustomer();
                            continueLoop = false;
                            break;

                        case "2":
                            continueLoop = false;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private static void QuitApplication()
        {
            Console.Clear();
            
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "Customers.txt")))
            {
                foreach (var customer in _customers)
                    outputFile.Write(customer.ToString());
            }

            //OSÄKER OM DETTA BEHÖVS
            /*string path = Path.Combine(docPath, "WriteLines.txt");

            int fileNumber = 0;

            while (File.Exists(path))
            {
                path = Path.Combine(docPath, $"WriteLines({fileNumber}).txt");
                fileNumber++;
            }*/

            _sessionState = SessionState.Terminate;
            _menuState = MenuState.Quit;
            Console.WriteLine("\n-----------Applikationen avslutades-----------");
        }

        public static void GenerateCustomer()
        {
            Console.Clear();
            Console.WriteLine("Vänligen skriv ditt önskade Användarnamn och sedan Lösenord.");
            Console.Write("Användarnamn: ");
            string newUsername = Console.ReadLine().Replace(" ", "");

            Console.Write("Lösenord: ");
            string newPassword = Console.ReadLine().Replace(" ", "");
            var newCustomer = new Customer(newUsername, newPassword);
            if (newUsername == "" || newPassword == "")
            {
                Console.WriteLine("Användarnamnet eller lösenordet är tomt. Försök igen.");
                Console.ReadLine();
                return;
            }
            foreach (var customer in _customers)
            {
                if (newUsername == customer.Username)
                {
                    Console.WriteLine($"Det finns redan en kund med användarnamnet \"{newUsername}\"");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    _customers.Add(newCustomer);
                    Console.WriteLine($"{newCustomer.Username} ha blivit registrerad.");
                    Console.ReadLine();
                    return;
                }
            }
            _customers.Add(newCustomer);
            Console.WriteLine($"{newCustomer.Username} har blivit registrerad som en ny kund.");
            Console.ReadLine();
            return;
        }
    }
}
