using Labb2test.Customers;
using Labb2test.Products;
using System;
using System.Collections.Generic;
using System.IO;

namespace Labb2test
{
    class Program
    {
        //Field variabler som man ofta behöver komma åt.
        private static MenuState _menuState;
        private static List<Customer> _allCustomers = new List<Customer>();
        private static Customer _loggedInCustomer;
        private static List<Product> _products = Product.GenerateListOfProducts();
        private static readonly string _docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static string _userChoosenCurrency = "SEK";

        //MenuState är det som bestämmer vilken meny som ska rendreras.
        enum MenuState
        {
            Quit, Welcome, LoggedIn, Buying
        }

        static void Main(string[] args)
        {
            //Hämta listan med sparade/fördefinierade kunder
            FetchSavedCustomers();
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            _menuState = MenuState.Welcome;

            //Så länge MenuState inte är Quit så ska denna "masterloop" loopas.
            while (_menuState != MenuState.Quit)
            {
                Console.Clear();

                //Gå in i rätt meny basaret på MenuState.
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

            //Kör en metod baserat på användarens input.
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
            Console.WriteLine($"Du är inloggad som \"{_loggedInCustomer.Username} ({_loggedInCustomer.Membership})\"" +
                $"\n--------------------------------------------------------" +
                $"\n1. Handla" +
                $"\n2. Se kundvagnen" +
                $"\n3. Gå till kassan och betala" +
                $"\n4. Logga ut");

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
            Console.WriteLine("Handla\n\nVälj något som du vill lägga till i kundvagnen. En vara åt gången. " +
                "\nDu kan byta valuta med genom att skriva SEK, EUR eller JPY\n");
            int itemNumber = 0;

            foreach (var product in _products)
            {
                itemNumber++;
                Console.Write($"{itemNumber}. {product.ToString(_userChoosenCurrency)}");
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

                case "SEK":
                    _userChoosenCurrency = "SEK";
                    break;

                case "EUR":
                    _userChoosenCurrency = "EUR";
                    break;

                case "JPY":
                    _userChoosenCurrency = "JPY";
                    break;

                default:
                    break;
            }
        }

        private static void Checkout()
        {
            Console.Clear();
            if (_loggedInCustomer.Cart.Count != 0)
            {
                double calculatedSum = _loggedInCustomer.CalculateSumBasedOnMembership(_loggedInCustomer.CartSumInSEK);
                Console.WriteLine($"Tack för köpet! " +
                    $"\nDu är en {_loggedInCustomer.Membership}-kund, så det kostade dig {calculatedSum}kr (istället för {_loggedInCustomer.CartSumInSEK}kr)");
                _loggedInCustomer.Cart.Clear();
                _loggedInCustomer.CartSumInSEK = 0;
                Console.ReadLine();
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

            foreach (var product in _loggedInCustomer.Cart)
            {
                if (product.ProductQuantity != 0)
                {
                    Console.Write(product.ToString());
                }
            }

            Console.WriteLine($"\nDen totala summan i SEK är: {_loggedInCustomer.CartSumInSEK} kr\n");

            Console.WriteLine($"I Euro: {Product.ConvertSumPriceInEUR(_loggedInCustomer.CartSumInSEK)}");
            Console.WriteLine($"I Yen: {Product.ConvertSumPriceInJPY(_loggedInCustomer.CartSumInSEK)}");
            double calculatedSum = _loggedInCustomer.CalculateSumBasedOnMembership(_loggedInCustomer.CartSumInSEK);
            Console.WriteLine($"\nMed {_loggedInCustomer.Membership}-medlemskap kostar det: {calculatedSum} kr");

            Console.WriteLine("\nTryck ENTER för att gå tillbaka.");
            Console.ReadLine();
        }

        private static void AddProductToCart(string userInput)
        {
            Product newProductToCart;
            newProductToCart = new Product(_products[int.Parse(userInput) - 1].ProductName, _products[int.Parse(userInput) - 1].ProductPrice);

            int loopCount = 0;

            //För varje produkt i kundens kundvagn
            foreach (var product in _loggedInCustomer.Cart)
            {
                //om det finns en produkt med samma namn i kundvagnen
                if (product.ProductName == newProductToCart.ProductName)
                {
                    //ge lägg till en till sån (+1 quantity på denna produkt)
                    product.ProductQuantity += 1;
                    //Lägg till den tillagda produktens pris till totalsumman.
                    _loggedInCustomer.CartSumInSEK += newProductToCart.ProductPrice;
                    break;
                }
                loopCount++;
            }
            //om loopen har gåt igenom alla kunder utan att hitta en produkt med samma namn som den man vill lägga till
            if (loopCount == _loggedInCustomer.Cart.Count)
            {
                //lägg till den som en ny produkt.
                _loggedInCustomer.Cart.Add(newProductToCart);
                _loggedInCustomer.CartSumInSEK += newProductToCart.ProductPrice;
            }

            //tror inte detta behövs, man kommer aldrig komma hit med Cart.Count = 0. (Om loopCount och Cart.Count = 0 så går den in i den ovanför.)
            else if (_loggedInCustomer.Cart.Count == 0)
            {
                _loggedInCustomer.Cart.Add(newProductToCart);
                _loggedInCustomer.CartSumInSEK += newProductToCart.ProductPrice;
            }
        }

        private static void LoginCustomer()
        {
            //While loop som gör så att användaren kan välja "försök igen" på vissa ställen.
            bool tryAgain = true;
            while (tryAgain)
            {
                Console.Clear();

                //Be om användarnamn / löseord
                Console.WriteLine("Logga in\n");
                Console.Write("Skriv in ditt användarnamn: ");
                string loginUsername = Console.ReadLine();

                Console.Write("Skriv in ditt lösenord: ");
                string loginPassword = Console.ReadLine();

                //Kolla om uppgifterna stämmer
                bool loginSuccess = CheckIfLoginSuccess(loginUsername, loginPassword);

                //Om allt stämmer överens,
                if (loginSuccess)
                { 
                    //Ändra MenuState till till LoggedIn
                    Console.Write($"\nDu är nu inloggad som \"{_loggedInCustomer.Username}\"!!");
                    Console.ReadLine();
                    _menuState = MenuState.LoggedIn;
                    return;
                }
                else
                {
                    //Annars, fråga om användaren vill registrera sig, försöka igen eller gå tillbaka
                    Console.Clear();
                    Console.WriteLine($"Kunden {loginUsername} finns inte i våran databas, " +
                        $"eller så har du skrivit fel." +
                        $"\nVill du registrera dig som ny kund eller försöka igen?");

                    Console.WriteLine("\n1. Registrera som ny kund" +
                                      "\n2. Försök igen " +
                                      "\n3. Gå tillbaka till huvudmenyn");

                    //Loop så att användaren måste välja 1,2 eller 3.
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
            //För varje registrerad kund, kolla om användarnamnet och lösenordet stämmer överns.
            foreach (var customer in _allCustomers)
            {   
                //Om det stämmer, för så att den matchade kunden är den inloggade kunden.
                if (Customer.VerifyPassword(loginPassword, customer) && loginUsername == customer.Username)
                {
                    _loggedInCustomer = customer;
                    return true;
                }
            }
            return false;
        }

        public static void GenerateCustomer()
        {
            Console.Clear();
            Console.WriteLine("Vänligen skriv ditt önskade Användarnamn och sedan Lösenord, " +
                "samt vilket medlemskap du vill ha." +
                "\n(Brons = 5% rabatt, Silver = 10% rabatt, Guld = 15% rabbat.\n");
            Console.Write("Användarnamn: ");
            string newUsername = Console.ReadLine().Replace(" ", "");
            bool isDuplicateUsername = CheckIfUsernameExists(newUsername);
            if (isDuplicateUsername)
            {
                Console.WriteLine($"\nDet finns redan en kund med användarnamnet \"{newUsername}\". " +
                    $"Försök igen med ett annat användarnamn.");
                Console.ReadLine();
                return;
            }

            Console.Write("Lösenord: ");
            string newPassword = Console.ReadLine().Replace(" ", "");

            Console.WriteLine("\nVälj medlemskap:" +
                "\n1. Brons (5% rabatt)" +
                "\n2. Silver (10% rabatt)" +
                "\n3. Guld(15% rabatt)\n");

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

            else if (!isDuplicateUsername)
            {
                AddNewCustomerBasedOnMembership(newUsername, newPassword, newMembership);
            }
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

        private static void LogoutCustomer()
        {
            Console.Clear();
            Console.WriteLine($"Du loggades ut {_loggedInCustomer.Username} ({_loggedInCustomer.Membership})");
            _loggedInCustomer = null;
            _menuState = MenuState.Welcome;
            Console.ReadLine();
        }

        private static void FetchSavedCustomers()
        {
            //Om det inte finns en textfil med sparade/fördefinierade kunder
            if (!File.Exists(_docPath))
            {
                //Skapa en textfil
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "CustomersViktor.txt")))
                {
                    //och lägg till de föredefinierade kunderna (Knatte, Fnatte, Tjatte) i en lista
                    var predefinedCustomers = new List<Customer>() { new GoldCustomer("Knatte", "123", Membership.Gold),
                                                                     new SilverCustomer("Fnatte", "321", Membership.Silver),
                                                                     new BronzeCustomer("Tjatte", "213", Membership.Bronze), };

                    //för varje fördefinierad kund, spara den i textfilen
                    foreach (var customer in predefinedCustomers)
                    {
                        outputFile.Write(SaveUser(customer));
                    }
                }
            }

            //Läs in texfilen med kunder
            using (var sr = new StreamReader(Path.Combine(_docPath, "CustomersViktor.txt")))
            {
                //Splitta texten på ett specifikt tecken (borde nog vara CSV (;))
                var text = sr.ReadToEnd();
                string[] splitText = text.Split('鯨');

                //För varje split
                for (int i = 0; i < splitText.Length - 1; i += 3)
                {
                    //spara undan namn, lösenord och medlemskap
                    string savedUsername = splitText[i];
                    string savedPassword = splitText[i + 1];
                    Membership savedMembership = Enum.Parse<Membership>(splitText[i + 2]);

                    //Baserat på kundens medlemskap, lägg till i en lista för alla kunder
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

        //Sparar en specifik kund till textfilen med kunder.
        public static string SaveUser(Customer customer)
        {
            string password = Customer.GetPassword(customer);
            return String.Format($"{customer.Username}鯨{password}鯨{customer.Membership}鯨");
        }

        private static void QuitApplication()
        {
            Console.Clear();
            //Sparar undan alla registrerade kunder, sen stäng ner applikationen.
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "CustomersViktor.txt")))
            {
                foreach (var customer in _allCustomers)
                {
                    outputFile.Write(SaveUser(customer));
                }
            }
            _menuState = MenuState.Quit;
            Console.WriteLine("\n-----------Applikationen avslutades-----------");
        }
    }
}