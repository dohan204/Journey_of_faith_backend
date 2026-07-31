using Journey_of_faith.Api.dtos;
using Journey_of_faith.Application.common.dtos;
using Journey_of_faith.Application.usecases.dashboard.queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashBoardesController : ControllerBase
{
    private readonly IMediator _mediator;
    public DashBoardesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCardHeaderInfo()
    {
        var result = await _mediator.Send(new GetDashboardQuery());
        return Ok(new ApiResponse<DashboardInfoDto>
        {
            Message = "Lấy thông tin dashboard thành công.",
            Data = result
        });
    }
}