using Asp.Versioning;
using FirebaseAdmin.Messaging;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.usecases.notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("api/v{version:apiVersion}/notifications")]
public class NotificationController : ControllerBase
{
    private readonly IMediator mediator;
    public NotificationController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [MapToApiVersion(1)]
    [HttpPost("send-notification")]
    public async Task<IActionResult> PushToUser(string? token, string? topic, string title, string message)
    {
        try
        {
            var dataPayload = new Dictionary<string, string> { { "click_action", "OPEN_ARTICLE" }, { "articleId", "123" } };
            var messageId = await mediator.Send(new PushNotificationCommand
            {
                Topic = topic,
                Token = token,
                Title = title,
                Body = message,
                Data = dataPayload
            });

            return Ok(new {Success = true, MessageId = messageId});
        } catch (FirebaseMessagingException ex)
        {
            return BadRequest(new { Success = false, Error = ex.Message, ErrorCode = ex.MessagingErrorCode.ToString() });
      
        }
    }

}
