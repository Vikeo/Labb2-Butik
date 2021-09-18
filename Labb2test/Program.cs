using Labb2test.Customers;
using Labb2test.Products;
using System;
using System.Collections.Generic;
using System.IO;

namespace Labb2test
{
    class Program
    {
        private static MenuState _menuState;
        private static List<Customer> _allCustomers = new List<Customer>();
        private static Customer _loggedInCustomer;
        private static List<Product> _products = Product.GenerateListOfProducts();
        private static List<Product> _userCart = new List<Product>();
        private static readonly string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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

            //Session active
            while (_menuState != MenuState.Quit)
            {
                Console.Clear();
                //Menu1 Kanske borde ha if
                switch (_menuState)
                {
                    case MenuState.Welcome:
                        RenderWelcomeMenu();
                        break;
                    case MenuState.LoggedIn:
                        RenderLoggedinMenu();
                        break;
                    case MenuState.Buying:
                        RenderBuyMenu();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RenderWelcomeMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Logga in\n2. Registrera dig som ny kund\n3. Avsluta");
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
            Console.WriteLine($"Du är inloggad som \"{_loggedInCustomer.Username} ({_loggedInCustomer.Membership})\"\n--------------------------------------------------------\n1. Handla\n2. Se kundvagnen\n3. Gå till kassan\n4. Logga ut");
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
                case "2":
                case "3":
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
                _userCart.Clear();
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
            Console.WriteLine("Din kundvagn:");
            var itemCounter = 0;
            ;
            foreach (var product in _userCart)
            {
                Console.WriteLine(product);
                itemCounter++;
            }
            Console.WriteLine($"\nDen totala summan i SEK är: {_sumPriceInSEK}kr\n");

            Console.WriteLine($"I Euro: {ConvertSumPriceInEUR(_sumPriceInSEK)}");
            Console.WriteLine($"I Yen: {ConvertSumPriceInJPY(_sumPriceInSEK)}");

            var calculatedSum = 0d;

            calculatedSum = _loggedInCustomer.CalculateSumBasedOnMembership(_sumPriceInSEK);

            Console.WriteLine($"\nMed {_loggedInCustomer.Membership}-medlemskap kostar det: {calculatedSum}kr");

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
            bool tryAgain = true;
            while (tryAgain)
            {
                Console.Clear();
                Console.WriteLine("Logga in");
                Console.Write("Skriv in ditt användarnamn: ");
                string loginUsername = Console.ReadLine();
                Console.Write("Skriv in ditt lösenord: ");
                string loginPassword = Console.ReadLine();
                bool loginSuccess = CheckIfLoginSuccess(loginUsername, loginPassword);

                if (loginSuccess)
                {
                    Console.Write($"\nDu är nu inloggad som \"{_loggedInCustomer.Username}\"!!");
                    Console.ReadLine();
                    _menuState = MenuState.LoggedIn;
                    return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Kunden {loginUsername} finns inte i våran databas, eller så har du skrivit fel.\nVill du registrera dig som ny kund eller försöka igen?");
                    Console.WriteLine("\n1. Registrera som ny kund\n2. Försök igen \n3. Gå tillbaka till huvudmenyn");
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

                            case "3":
                                tryAgain = false;
                                continueLoop = false;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        private static bool CheckIfLoginSuccess(string loginUsername, string loginPassword)
        {
            foreach (var customer in _allCustomers)
            {
                if (loginUsername == customer.Username && loginPassword == customer.Password)
                {
                    _loggedInCustomer = customer;
                    return true;
                }
            }
            return false;
        }

        private static void FetchSavedCustomers()
        {
            using (var sr = new StreamReader(Path.Combine(_docPath, "Customers.txt")))
            {
                var text = sr.ReadToEnd();
                //var textLine = sr.ReadLine();   //Coolare/Bättre
                string[] splitText = text.Split('鯨'); //ändra till ,/; ??? (CSV)
                for (int i = 0; i < splitText.Length - 1; i += 3)
                {
                    string savedUsername = splitText[i];
                    string savedPassword = splitText[i + 1];
                    Membership savedMembership = Enum.Parse<Membership>(splitText[i + 2]);

                    switch (savedMembership)
                    {
                        case Membership.NonMember:
                            QuitApplication();
                            break;

                        case Membership.Bronze:
                            var bronzeCustomer = new BronzeCustomer(savedUsername, savedPassword, savedMembership);
                            _allCustomers.Add(bronzeCustomer);
                            break;

                        case Membership.Silver:
                            var silverCustomer = new SilverCustomer(savedUsername, savedPassword, savedMembership);
                            _allCustomers.Add(silverCustomer);
                            break;

                        case Membership.Gold:
                            var goldCustomer = new GoldCustomer(savedUsername, savedPassword, savedMembership);
                            _allCustomers.Add(goldCustomer);
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
            Console.WriteLine("Vänligen skriv ditt önskade Användarnamn och sedan Lösenord, samt vilket medlemskap du vill ha.\n(Brons = 5% rabatt, Silver = 10% rabatt, Guld = 15% rabbat.\n");
            Console.Write("Användarnamn: ");
            string newUsername = Console.ReadLine().Replace(" ", "");
            bool isDuplicateUsername = CheckIfUsernameExists(newUsername);
            if (isDuplicateUsername)
            {
                Console.WriteLine($"\nDet finns redan en kund med användarnamnet \"{newUsername}\". Försök igen med ett annat användarnamn.");
                Console.ReadLine();
                return;
            }

            Console.Write("Lösenord: ");
            string newPassword = Console.ReadLine().Replace(" ", "");

            Console.WriteLine("\nVälj medlemskap:\n1. Brons (5% rabatt)\n2. Silver (10% rabatt)\n3. Guld(15% rabatt)\n");
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
                Console.WriteLine("\nAnvändarnamnet eller lösenordet är tomt. Försök igen");
                Console.ReadLine();
                return;
            }

            else if(!isDuplicateUsername)
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
                    _allCustomers.Add(newBronzeCustomer);
                    break;

                case Membership.Silver:
                    var newSilverCustomer = new SilverCustomer(newUsername, newPassword, newMembership);
                    _allCustomers.Add(newSilverCustomer);
                    break;

                case Membership.Gold:
                    var newGoldCustomer = new GoldCustomer(newUsername, newPassword, newMembership);
                    _allCustomers.Add(newGoldCustomer);
                    break;

                default:
                    break;
            }
            Console.WriteLine($"{newUsername} ({newMembership}) har blivit registrerad som en ny kund.");
            Console.ReadLine();
        }

        private static bool CheckIfUsernameExists(string username)
        {
            foreach (var customer in _allCustomers)
            {
                if (username == customer.Username)
                {
                    return true;
                }
            }
            return false;
        }

        private static void LogoutCustomer()
        {
            Console.Clear();
            _userCart.Clear();
            _loggedInCustomer = null;
            _menuState = MenuState.Welcome;
            Console.WriteLine("Du loggades ut");
            Console.ReadLine();
        }

        private static void QuitApplication()
        {
            Console.Clear();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "Customers.txt")))
            {
                foreach (var customer in _allCustomers)
                    outputFile.Write(customer.ToString());
            }

            _userCart.Clear();
            _menuState = MenuState.Quit;
            Console.WriteLine("\n-----------Applikationen avslutades-----------");
        }
    }
}