using FirebaseAdmin.Messaging;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.notifications;
using Journey_of_faith.Infrastructure.context;
using Microsoft.EntityFrameworkCore;

namespace Journey_of_faith.Infrastructure.services;


public class FirebaseNotification : IFirebaseNotification
{
    private readonly ApplicationDbContext _dbContext;
    public FirebaseNotification(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<string> SendNotificationAsync(string deviceToken, string title, string body, Dictionary<string, string>? data = null)
    {
        var message = new Message()
        {
            Token = deviceToken, // token thiết bị nhận thông báo
            Notification = new Notification() // tiêu đề và nội dung của thông báo
            {
                Title = title, 
                Body = body
            },
            Data = data
        };


        return await FirebaseMessaging.DefaultInstance.SendAsync(message);
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

    public async Task FcmRegisterAsync(DeviceToken deviceToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.DeviceTokens.AddAsync(deviceToken, cancellationToken);
    }

    public async Task<bool> DeviceTokenExistsAsync(DeviceToken deviceToken, CancellationToken cancellationToken  = default)
    {
        return await _dbContext.DeviceTokens.AnyAsync(
            e => e.UserId == deviceToken.UserId && 
                e.Token != deviceToken.Token && 
                e.Platform == deviceToken.Platform,
            cancellationToken
        );
    }
}
