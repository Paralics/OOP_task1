using Microsoft.AspNetCore.SignalR;

public class VendingMachine
{
    private readonly string _key;
    // Ключ для перехода в режим админа
    private Inventory _innerInventory = new Inventory(new Dictionary<string, byte>(), new byte[Inventory.Currency.Count]);
    // Тут хранятся деньги и еда автомата
    public Dictionary<string, ushort> Prices { get; }
    // Прейскурант
    private bool _adminMode = false;
    // В режиме ли админа автомат
    private string? _selected { get; set; } = null;
    // Выбранная еда
    public Inventory OuterInventory = new Inventory(new Dictionary<string, byte>(), new byte[Inventory.Currency.Count]);
    // Тут хранится сдача и выданная еда
    public VendingMachine(string key, Dictionary<string, ushort> prices, Inventory? food_money = null)
    {
        Prices = prices;
        _key = key;
        if (!(food_money is null))
        {
            _innerInventory = food_money;
        }
    }
    public void ChangeMode(string key)
    {
        if (key.Equals(_key))
        {
            _adminMode = !_adminMode;
        }
        else
        {
            Console.WriteLine("Wrong key!");
        }
    }
    public void SeeFood()
    {
        _innerInventory.SeeInventorty(false, true);
    }
    public void SeePrices()
    {
        foreach (string item in Prices.Keys)
        {
            Console.WriteLine($"{item}: {Prices[item]}");
        }
    }
    public void GiveChange(ushort change)
    {
        // Считает и выдает сдачу монетками, если не может выдать сдачу - прерывает транакцию
        // Да, сдача корректно выдается/не выдается вседа только если для любых двух номиналов a<b GCD(a, b)=a
        byte[] change_count = new byte[Inventory.Currency.Count];
        for (int i = Inventory.Currency.Count - 1; i >= 0; i--)
        {
            ushort used = 0;
            while (Inventory.Currency[i] <= change & _innerInventory.Money[i] - used > 0)
            {
                change_count[i]++;
                used++;
                change -= Inventory.Currency[i];
            }
        }
        if (change > 0)
        {
            throw new NoChangeExeption();
        }

        _innerInventory.GiveMoney(change_count, OuterInventory);
    }
    public void SelectFood(string food_name)
    {
        if (!Prices.Keys.Contains(food_name))
        {
            throw new UnknownFoodException(food_name);
        }
        _selected = food_name;
        Console.WriteLine($"{food_name} successfully selected! Awaiting payment.");
    }
    public void BuyFood(Inventory buyer, byte[] money)
    {
        // Списывает выбранную сумму (если она не меньше цены и выдана монетками которые есть)
        // Выдает выбранную еду (если есть) и сдачу
        if (_selected is null)
        {
            throw new NothingSelectedException();
        }
        ushort total = 0;
        for (byte i = 0; i != money.Length; i++)
        {
            total += (ushort)(money[i] * Inventory.Currency[i]);
        }
        if (total < Prices[_selected])
        {
            throw new NotEnoughMoneyException();
        }
        buyer.GiveMoney(money, _innerInventory);
        GiveChange((ushort)(total - Prices[_selected]));
        _innerInventory.GiveFood(new Tuple<string, byte>(_selected, 1), OuterInventory);
    }
    public void CollectChange(Inventory buyer)
    {
        // Отдает сдачу из коробки для сдачи покупателю
        OuterInventory.GiveMoney(OuterInventory.Money, buyer);
    }
    public void CollectFood(Inventory buyer)
    {
        // Отдает еду из коробки для выданной еды покупателю
        foreach (string food in OuterInventory.Food.Keys)
        {
            OuterInventory.GiveFood(new Tuple<string, byte>(food, OuterInventory.Food[food]), buyer);
        }
    }
    public void AddFood(Inventory admin, Tuple<string, byte> food)
    {
        // Забирает у админа указанное количество указанного типа еды, если ее достаточно
        if (!_adminMode)
        {
            throw new WrongModeException();
        }
        if (!Prices.Keys.Contains(food.Item1))
        {
            throw new UnknownFoodException(food.Item1);
        }
        admin.GiveFood(food, _innerInventory);
    }
    public void SeeInventory()
    {
        if (!_adminMode)
        {
            throw new WrongModeException();
        }
        _innerInventory.SeeInventorty();
    }
    public void CollectMoney(Inventory admin, byte[] coins)
    {
        // Дает админу деньги
        if (!_adminMode)
        {
            throw new WrongModeException();
        }
        _innerInventory.GiveMoney(coins, admin);
    }
    public void GiveMoney(Inventory admin, byte[] coins)
    {
        // Забирает у админа деньги
        if (!_adminMode)
        {
            throw new WrongModeException();
        }
        admin.GiveMoney(coins, _innerInventory); 
    }
}