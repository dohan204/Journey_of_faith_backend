using Journey_of_faith.Application.common.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.notifications;


public class PushNotificationCommand : IRequest<string>
{
    public string Title {get; set;}
    public string Body {get; set;}
    public string? Token {get; set;}
    public string? Topic {get; set;}
    public Dictionary<string, string>? Data {get; set;}
}



public class PushNotificationHandler : IRequestHandler<PushNotificationCommand, string>
{
    private readonly IFirebaseNotification notification;
    public PushNotificationHandler(IFirebaseNotification notification)
    {
        this.notification = notification;
    }

    public async Task<string> Handle(PushNotificationCommand command, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(command.Token))
        {
            return await notification.SendNotificationAsync(command.Token, command.Title, command.Body);
        } else
        {
            return await notification.SendToTopicAsync(command.Topic, command.Title, command.Body, command.Data);
        }
    }
}