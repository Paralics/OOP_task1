public class NotEnoughMoneyException : Exception
{
    // Ошибка для некорректной суммы при оплате
    public NotEnoughMoneyException() : base("Not enough money :(")
    {
    }
}

public class NotEnoughCoinsException : Exception
{
    // Ошибка для некорректного числа монет для передачи
    public NotEnoughCoinsException(byte asked, byte got, ushort type) : base($"Not enough coins: asked for {asked} {type}s got only {got} :(")
    {
    }
}
public class UnknownFoodException : Exception
{
    // Ошибка для попытки заказать или поместить еду, которую автомат не продает
    public UnknownFoodException(string food_name) : base($"The machine doesn't sell {food_name}")
    {
    }
}

public class NotEnoughFoodException : Exception
{
    // Ошибка для попытки дать больше еды чем есть
    public NotEnoughFoodException(string food_name) : base($"Not enough {food_name}s")
    {
    }
}

public class WrongModeException : Exception
{
    // Ошибка доступа
    public WrongModeException() : base("Acces denied")
    {
    }
}

public class NoChangeExeption : Exception
{
    // Ошибка нехватки сдачи
    public NoChangeExeption() : base("The machine has no change. Transaction canceled.")
    {
    }
}

public class NothingSelectedException : Exception
{
    // Ошибка отсутствия выбранной еды
    public NothingSelectedException() : base("No food is selected!")
    {
    }
}