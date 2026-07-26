using FirebaseAdmin.Messaging;
using Journey_of_faith.Application.common.interfaces;

namespace Journey_of_faith.Infrastructure.services;


public class FirebaseNotification : IFirebaseNotification
{
    public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body)
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
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            return !string.IsNullOrEmpty(response);
        } catch
        {
            return false;
        }
    }
}