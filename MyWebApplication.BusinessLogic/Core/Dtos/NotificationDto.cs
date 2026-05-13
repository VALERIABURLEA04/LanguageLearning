namespace MyWebApplication.BusinessLogic.Core.Dtos;

public record NotificationDto(int Id, string Message, bool IsRead, System.DateTime CreatedAt);
