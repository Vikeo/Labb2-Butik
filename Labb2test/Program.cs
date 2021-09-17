using Labb2test.Customers;
using Labb2test.Products;
using System;
using System.Collections.Generic;
using System.IO;

namespace Labb2test
{
    class Program
    {
        private static SessionState _sessionState;
        private static MenuState _menuState;
        private static List<Customer> _allCustomers = new List<Customer>();
        private static List<GoldCustomer> _goldCustomers = new List<GoldCustomer>();
        private static List<SilverCustomer> _silverCustomers = new List<SilverCustomer>();
        private static List<BronzeCustomer> _bronzeCustomers = new List<BronzeCustomer>();
        private static string _loggedInCustomer = "N/A";
        private static Membership _loggedInCustomerMembership = Membership.NonMember;
        private static readonly string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static List<Product> _products = Product.GenerateListOfProducts();
        private static List<Product> _userCart = new List<Product>();
        private static double _sumPriceInSEK = 0;

        enum MenuState
        {
            Quit, Welcome, LoggedIn, Buying
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

                while (_menuState == MenuState.Buying)
                {
                    RenderBuyMenu();
                }
            }
        }

        private static void RenderWelcomeMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Logga in\n2. Registrera dig som ny kund\n3. Avsluta");
            UpdateAllCustomersList();
            Console.WriteLine($"\nAntal registrerade kunder: {_allCustomers.Count}");

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

        private static void RenderLoggedinMenu()
        {
            Console.Clear();
            Console.WriteLine($"Du är inloggad som \"{_loggedInCustomer} ({_loggedInCustomerMembership})\"\n--------------------------------------------------------\n1. Handla\n2. Se kundvagnen\n3. Gå till kassan\n4. Logga ut");
            switch (Console.ReadLine())
            {
                case "1":
                    _menuState = MenuState.Buying;
                    break;

                case "2":
                    PrintCart();
                    break;

                case "3":
                    Checkout();
                    break;

                case "4":
                    LogoutCustomer();
                    break;
    
                default:
                    break;
            }
        }
   
        private static void RenderBuyMenu()
        {
            Console.Clear();
            Console.WriteLine("Handla\nVälj något som du vill lägga till i kundvagnen. En vara åt gången.");
            int itemNumber = 0;
            foreach (var product in _products)
            {
                itemNumber++;
                Console.WriteLine($"{itemNumber}. {product}");
            };
            
            Console.WriteLine($"{itemNumber + 1}. Gå tillbaka");
            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    AddProductToCart(userInput);
                    break;

                case "2":
                    AddProductToCart(userInput);
                    break;

                case "3":
                    AddProductToCart(userInput);
                    break;

                case "4":
                    AddProductToCart(userInput);
                    break;

                case "5":
                    _menuState = MenuState.LoggedIn;
                    break;

                default:
                    break;
            }
        }

        private static void Checkout()
        {
            Console.Clear();
            if (_userCart.Count != 0)
            {
                Console.WriteLine("Tack för köpet!");
                Console.ReadLine();
                QuitApplication();
                //Avsluta eller skicka tillbaka???
            }
            else
            {
                Console.WriteLine("Lägg till något i kundvagnen först");
                Console.ReadLine();
            }
        }

        private static void PrintCart()
        {
            Console.Clear();
            var itemCounter = 0;
            ;
            foreach (var product in _userCart)
            {
                Console.WriteLine(product);
                itemCounter++;
            }
            Console.WriteLine($"\nDen totala summan i SEK är: {_sumPriceInSEK}:-\n");

            Console.WriteLine($"I Euro: {ConvertSumPriceInEUR(_sumPriceInSEK)}");
            Console.WriteLine($"I Yen: {ConvertSumPriceInJPY(_sumPriceInSEK)}");

            var calculatedSum = 0d;
            switch (_loggedInCustomerMembership)
            {
                case Membership.NonMember:
                    break;

                case Membership.Bronze:
                    calculatedSum = ;
                    break;

                case Membership.Silver:
                    calculatedSum = SilverCustomer.CalculateSumBasedOnMembership();
                    break;

                case Membership.Gold:
                    calculatedSum = GoldCustomer.CalculateGoldCustomerSum(_sumPriceInSEK);
                    break;

                default:
                    break;
            }

            Console.WriteLine($"\n");

            Console.WriteLine("\nTryck ENTER för att gå tillbaka.");


            //Brons  Medlem: 5% rabatt på hela köpet.
            //Silver Medlem: 10% rabatt på hela köpet.
            //Guld Medlem:  15% rabatt på hela köpet.

            Console.ReadLine();
        }

        //DRY??????
        private static double ConvertSumPriceInEUR(double sumInSEK)
        {
            var conversionRate = 0.0984775f;
            var sumInEURO = sumInSEK * conversionRate;
            return Math.Round(sumInEURO, 2);
        }
        private static double ConvertSumPriceInJPY(double sumInSEK)
        {
            var conversionRate = 12.7529f;
            var sumInYEN = sumInSEK * conversionRate;
            return Math.Round(sumInYEN, 2);
        }

        private static void AddProductToCart(string userInput)
        {
            Product productToCart;
            productToCart = new Product(_products[int.Parse(userInput) - 1].ProductName, _products[int.Parse(userInput) - 1].ProductPrice);
            _userCart.Add(productToCart);
            _sumPriceInSEK += _products[int.Parse(userInput) - 1].ProductPrice;
        }

        private static void LoginCustomer()
        {
            UpdateAllCustomersList();
            bool tryAgain = true;
            bool usernameMatch = false;
            while (tryAgain)
            {
            Console.Clear();
            Console.WriteLine("Logga in");
            Console.Write("Skriv in ditt användarnamn: ");
            string loginUsername = Console.ReadLine();
            
                foreach (var customer in _allCustomers)
                {
                    if (loginUsername == customer.Username)
                    {
                        Console.Write("Skriv in ditt lösenord: ");
                        string loginPassword = Console.ReadLine();
                        usernameMatch = true;
                        if (loginPassword == customer.Password)
                        {
                            Console.Write($"\nDu är nu inloggad som \"{customer.Username}\"!!");
                            Console.ReadLine();
                            _menuState = MenuState.LoggedIn;
                            _loggedInCustomerMembership = customer.Membership;
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
                        }
                    }
                }
                if (!usernameMatch)
                {
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
        }

        private static void FetchSavedCustomers()
        {
            using (var sr = new StreamReader(Path.Combine(_docPath, "Customers.txt")))
            {
                var text = sr.ReadToEnd();
                string[] splitText = text.Split('鯨');
                for (int i = 0; i < splitText.Length - 1; i += 3)
                {
                    string savedUsername = splitText[i];
                    string savedPassword = splitText[i + 1];
                    Membership savedMembership = Enum.Parse<Membership>(splitText[i + 2]);
                    var savedCustomer = new GoldCustomer(savedUsername, savedPassword, savedMembership);
                    switch (savedMembership)
                    {
                        case Membership.NonMember:
                            QuitApplication();
                            break;

                        case Membership.Bronze:
                            _goldCustomers.Add(savedCustomer);
                            break;

                        case Membership.Silver:
                            _goldCustomers.Add(savedCustomer);
                            break;

                        case Membership.Gold:
                            _goldCustomers.Add(savedCustomer);
                            break;

                        default:
                            break;
                    }
                    
                }
            }
        }

        public static void GenerateCustomer()
        {
            Console.Clear();
            Console.WriteLine("Vänligen skriv ditt önskade Användarnamn och sedan Lösenord, samt vilket medlemskap du vill ha.\n(Brons = 5% rabatt, Silver = 10% rabatt, Guld = 15% rabbat.");
            Console.Write("Användarnamn: ");
            string newUsername = Console.ReadLine().Replace(" ", "");

            Console.Write("Lösenord: ");
            string newPassword = Console.ReadLine().Replace(" ", "");
            
            //bool success = Enum.TryParse<Membership>(Console.ReadLine(), out newMembership);
            //if (!success)
            //{
            //    Console.WriteLine("Du måste välja ett medlemskap.");
            //    return;
            //}
            Console.WriteLine("Välj medlemskap:\n1. Brons (5% rabatt)\n2. Silver (10% rabatt)\n3. Guld(15% rabatt)\n");
            Membership newMembership = Membership.NonMember;
            bool continueLoop = true;
            while (continueLoop)
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        newMembership = Membership.Bronze;
                        continueLoop = false;
                        break;

                    case "2":
                        newMembership = Membership.Silver;
                        continueLoop = false;
                        break;

                    case "3":
                        newMembership = Membership.Gold;
                        continueLoop = false;
                        break;

                    default:
                        break;
                }
            }

            if (newUsername == "" || newPassword == "")
            {
                Console.WriteLine("Användarnamnet eller lösenordet är tomt. Försök igen.");
                Console.ReadLine();
                return;
            }
            
            bool isDuplicateUsername = CheckIfUsernameIsTaken(newUsername);
            if (!isDuplicateUsername)
            {
                AddNewCustomerBasedOnMembership(newUsername, newPassword, newMembership);
            }
            return;
        }

        private static void AddNewCustomerBasedOnMembership(string newUsername, string newPassword, Membership newMembership)
        {
            switch (newMembership)
            {
                case Membership.NonMember:
                    break;

                case Membership.Bronze:
                    var newBronzeCustomer = new BronzeCustomer(newUsername, newPassword, newMembership);
                    _bronzeCustomers.Add(newBronzeCustomer);
                    break;

                case Membership.Silver:
                    var newSilverCustomer = new SilverCustomer(newUsername, newPassword, newMembership);
                    _silverCustomers.Add(newSilverCustomer);
                    break;

                case Membership.Gold:
                    var newGoldCustomer = new GoldCustomer(newUsername, newPassword, newMembership);
                    _goldCustomers.Add(newGoldCustomer);
                    break;

                default:
                    break;
            }
            Console.WriteLine($"{newUsername} ({newMembership}) har blivit registrerad som en ny kund.");
            Console.ReadLine();
        }

        private static void UpdateAllCustomersList()
        {
            _allCustomers.Clear();
            _allCustomers.AddRange(_bronzeCustomers);
            _allCustomers.AddRange(_silverCustomers);
            _allCustomers.AddRange(_goldCustomers);
        }

        private static bool CheckIfUsernameIsTaken(string newUsername)
        {
            UpdateAllCustomersList();
            foreach (var customer in _allCustomers)
            {
                if (newUsername == customer.Username)
                {
                    Console.WriteLine($"Det finns redan en kund med användarnamnet \"{newUsername}\"");
                    Console.ReadLine();
                    return true;
                }
            }
            return false;
        }

        private static void LogoutCustomer()
        {
            Console.Clear();
            _loggedInCustomer = "N/A";
            _menuState = MenuState.Welcome;
            Console.WriteLine("Du loggades ut");
            Console.ReadLine();
        }

        private static void QuitApplication()
        {
            Console.Clear();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "Customers.txt")))
            {
                foreach (var customer in _goldCustomers)
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
            _userCart.Clear();
            _sessionState = SessionState.Terminate;
            _menuState = MenuState.Quit;
            Console.WriteLine("\n-----------Applikationen avslutades-----------");
        }
    }
}