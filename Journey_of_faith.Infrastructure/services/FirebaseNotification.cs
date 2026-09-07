using FirebaseAdmin.Messaging;
using Journey_of_faith.Application.common.interfaces;

namespace Journey_of_faith.Infrastructure.services;


public class FirebaseNotification : IFirebaseNotification
{
    public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
    {
        var message = new Message()
        {
            Token = deviceToken, // token thiết bị nhận thông báo
            Notification = new Notification() // tiêu đề và nội dung của thông báo
            {
                Title = title, 
                Body = body
            }
        };


        try
        {
            // sử dụng thông báo chính thức của firebase 
            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        } catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<string> SendToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null)
    {
        var message = new Message()
        {
            Topic = topic,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },
            Data = data
        };

        return await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
}