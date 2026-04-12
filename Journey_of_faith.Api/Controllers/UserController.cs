using Journey_of_faith.Application.usecases.users.commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                message = "đăng ký thành công",
                success = true
            });
        }
    }
}
