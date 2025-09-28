class Program
{
    static Dictionary<string, ushort> prices = new Dictionary<string, ushort>()
    {
        {"coke", 65},
        {"cookie", 150},
        {"protein bar", 300},
        {"atarax", 1000}
    };
    static Inventory starting_machine = new Inventory(new Dictionary<string, byte>()
    {
        {"atarax", 15},
        {"cookie", 200},
        {"protein bar", 5}
    }, new byte[] { 50, 50, 50, 50, 50 });
    private static VendingMachine _machine = new VendingMachine("0=#", prices, starting_machine);
    private static Inventory _player = new Inventory(null, new byte[] { 30, 12, 12, 2, 1 });
    // Стартовые состояния игрока и машины
    // Стоит думать очень плохо что я тут захардкодил валюты когда всю прошлую жизнь ссылался на Inventory.Currency
    // Но как, сказал классик, что уж тут поделаешь
    static void RunApp()
    {
        Console.WriteLine("You stand before a vending machine");
        Console.WriteLine("What Will you do? \n");
        bool admin_mode = false;
        while (true)
        {
            try
            {
                if (admin_mode)
                {
                    Console.WriteLine("The machine is in admin mode, you can: \nLock; See machine inventory; Collect money; Add money for change; Replenish food; Lock; See inventory; Skedaddle");
                    string input = Console.ReadLine();
                    switch (input.ToLower())
                    {
                        case "skedaddle":
                            Console.WriteLine("You leave the machine unlocked. A terrible idea, but you do you I guess.");
                            return;
                        case "see inventory":
                            _player.SeeInventorty();
                            break;
                        case "replenish food":
                            Console.WriteLine("What food do you add?");
                            string name = Console.ReadLine();
                            Console.WriteLine("How much?");
                            byte amount = byte.Parse(Console.ReadLine());
                            Tuple<string, byte> food = new Tuple<string, byte>(name, amount);
                            _machine.AddFood(_player, food);
                            Console.WriteLine("Food succesfully added!");
                            break;
                        case "see machine inventory":
                            _machine.SeeInventory();
                            break;
                        case "add money for change":
                            byte[] placed = new byte[Inventory.Currency.Count];
                            for (byte i = 0; i < 5; i++)
                            {
                                Console.WriteLine($"How many {Inventory.Currency[i]}s do you place?");
                                placed[i] = byte.Parse(Console.ReadLine());
                            }
                            _machine.GiveMoney(_player, placed);
                            break;
                        case "collect money":
                            byte[] taken = new byte[Inventory.Currency.Count];
                            for (byte i = 0; i < 5; i++)
                            {
                                Console.WriteLine($"How many {Inventory.Currency[i]}s do you take?");
                                taken[i] = byte.Parse(Console.ReadLine());
                            }
                            _machine.CollectMoney(_player, taken);
                            break;
                        case "lock":
                            Console.WriteLine("Insert key");
                            string key = Console.ReadLine();
                            _machine.ChangeMode(key);
                            admin_mode = false;
                            break;
                        default:
                            Console.WriteLine("Unknown action");
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("\nthe machine is operational. You can:\nSee wares; See prices; Select; Add payment; See inventory; Unlock; Collect food; Collect change; Skedaddle");
                    string input = Console.ReadLine();
                    switch (input.ToLower())
                    {
                        case "unlock":
                            Console.WriteLine("Insert key");
                            string key = Console.ReadLine();
                            _machine.ChangeMode(key);
                            admin_mode = true;
                            break;
                        case "see wares":
                            _machine.SeeFood();
                            break;
                        case "see prices":
                            _machine.SeePrices();
                            break;
                        case "skedaddle":
                            Console.WriteLine("You leave the machine withou saying goodbye. How very rude of you.");
                            return;
                        case "see inventory":
                            _player.SeeInventorty();
                            break;
                        case "select":
                            Console.WriteLine("What nourishment is to your liking?");
                            string food_name = Console.ReadLine();
                            _machine.SelectFood(food_name);
                            break;
                        case "add payment":
                            byte[] payment = new byte[Inventory.Currency.Count];
                            for (byte i = 0; i < 5; i++)
                            {
                                Console.WriteLine($"How many {Inventory.Currency[i]}s shall you place?");
                                payment[i] = byte.Parse(Console.ReadLine());
                            }
                            _machine.BuyFood(_player, payment);
                            Console.WriteLine("Transaction succesfull! Make sure to collect the food and the change");
                            break;
                        case "collect food":
                            _machine.CollectFood(_player);
                            Console.WriteLine("You succesfully collected the food");
                            break;
                        case "collect change":
                            _machine.CollectChange(_player);
                            Console.WriteLine("You succesfully collected the change");
                            break;
                        default:
                            Console.WriteLine("Unknown action");
                            break;
                    }
                }
            }
            catch (NoChangeExeption ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NotEnoughCoinsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NotEnoughFoodException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NotEnoughMoneyException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnknownFoodException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (NothingSelectedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (WrongModeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("Whe asked for a number, write a number pls >:D");
            }
            // Ловлю все задуманные ошибки

        }

    }
    static void Main(string[] args)
    {
        RunApp();
    } 
}