namespace Journey_of_faith.Application.common.interfaces;

public interface IFirebaseNotification
{
    Task<bool> SendNotificationAsync(string deviceToken, string title, string body);
}