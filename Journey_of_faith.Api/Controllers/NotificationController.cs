using Journey_of_faith.Application.common.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers;

public class NotificationController : ControllerBase
{
    private readonly IFirebaseNotification firebaseNotification;

    public NotificationController(IFirebaseNotification firebaseNotification)
    {
        this.firebaseNotification = firebaseNotification;
    }
    [HttpPost("test")]
    public async Task<IActionResult> Test([FromBody] TestNotificationDto dto)
    {
        var success = await firebaseNotification.SendNotificationAsync(
            dto.DeviceToken, dto.Title, dto.Body
        );

        return success ? Ok("sent") : BadRequest("failed");
    }
}


public class TestNotificationDto
{
    public string DeviceToken { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}