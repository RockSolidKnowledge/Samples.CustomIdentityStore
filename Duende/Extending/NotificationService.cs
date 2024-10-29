namespace Extending;

public interface INotificationService
{
    void Dispatch(string eventName);
}

public class NotificationService : INotificationService
{
    public void Dispatch(string eventName)
    {
        Console.WriteLine($"{eventName} logged!");
    }
}