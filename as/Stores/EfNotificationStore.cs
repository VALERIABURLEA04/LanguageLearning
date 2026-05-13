using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.BusinessLogic.Core.Dtos;
using MyWebApplication.BusinessLogic.Interfaces;

namespace sa.Stores;

public class EfNotificationStore : INotificationStore
{
    private readonly ApplicationDbContext _db;
    public EfNotificationStore(ApplicationDbContext db) => _db = db;

    public void Create(string message)
    {
        _db.Notifications.Add(new sa.Models.Notification { Message = message, CreatedAt = System.DateTime.UtcNow });
        _db.SaveChanges();
    }

    public IReadOnlyList<NotificationDto> ListAll() =>
        _db.Notifications
           .OrderByDescending(n => n.CreatedAt)
           .Select(n => new NotificationDto(n.Id, n.Message, n.IsRead, n.CreatedAt))
           .ToList();

    public int CountUnread() => _db.Notifications.Count(n => !n.IsRead);

    public void MarkAllRead()
    {
        _db.Notifications.Where(n => !n.IsRead).ExecuteUpdate(s => s.SetProperty(n => n.IsRead, true));
    }
}
