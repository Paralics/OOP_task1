public class Inventory
{
    public static List<ushort> Currency { get; } = new List<ushort> { 1, 5, 10, 100, 1000 };
    // Такие бывают монетки
    public byte[] Money { get; } = new byte[Currency.Count];
    // Кошелек
    public Dictionary<string, byte> Food { get; } = new Dictionary<string, byte>();
    // Инвентарь еды
    public Inventory(Dictionary<string, byte>? food = null, byte[]? coins = null)
    {
        if (!(food is null))
        {
            Food = food;
        }
        if (!(coins is null))
        {
            Money = coins;
        }
    }
    public void GiveFood(Tuple<string, byte> food, Inventory recipient)
    {

        if (!Food.Keys.Contains(food.Item1))
        {
            throw new NotEnoughFoodException(food.Item1);
        }
        if (Food[food.Item1] < food.Item2)
        {
            throw new NotEnoughFoodException(food.Item1);
        }
        Food[food.Item1] -= food.Item2;
        if (recipient.Food.Keys.Contains(food.Item1))
        {
            recipient.Food[food.Item1] += food.Item2;
        }
        else
        {
            recipient.Food.Add(food.Item1, food.Item2);
        }
        if (Food[food.Item1] == 0)
        {
            Food.Remove(food.Item1);
        }
    }
    public void GiveMoney(byte[] coins, Inventory recipient)
    {
        for (byte i = 0; i != coins.Length; i++)
        {
            if (Money[i] < coins[i])
            {
                throw new NotEnoughCoinsException(coins[i], Money[i], Currency[i]);
            }
            recipient.Money[i] += coins[i];
            Money[i] -= coins[i];
        }

    }
    public void SeeInventorty(bool show_money = true, bool show_food = true)
    { if (show_money)
        {
            Console.WriteLine("Money:");
            for (byte i = 0; i < Money.Length; i++)
            {
                Console.WriteLine($"{Money[i]} {Currency[i]}s");
            }
        }
        if (show_food)
        {
            Console.WriteLine("Food:");
            foreach (string item in Food.Keys)
            {
                Console.WriteLine($"{Food[item]} {item}s");
            }
        }
    }

}