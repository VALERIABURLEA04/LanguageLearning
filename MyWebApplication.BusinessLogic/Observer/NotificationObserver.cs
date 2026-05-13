using MyWebApplication.BusinessLogic.Interfaces;

namespace MyWebApplication.BusinessLogic.Observer;

public class NotificationObserver : IObserver
{
    private readonly INotificationStore _store;
    public NotificationObserver(INotificationStore store) => _store = store;

    public void Update(string message) => _store.Create(message);
}
