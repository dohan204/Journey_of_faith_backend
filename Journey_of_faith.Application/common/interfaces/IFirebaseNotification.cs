namespace Journey_of_faith.Application.common.interfaces;

public interface IFirebaseNotification
{
    Task<string> SendNotificationAsync(string deviceToken, string title, string body);
    Task<string> SendToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null);
}