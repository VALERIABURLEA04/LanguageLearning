using System.Collections.Generic;
using MyWebApplication.BusinessLogic.Core.Dtos;

namespace MyWebApplication.BusinessLogic.Interfaces;

public interface INotificationStore
{
    void                          Create(string message);
    IReadOnlyList<NotificationDto> ListAll();
    int                           CountUnread();
    void                          MarkAllRead();
}
