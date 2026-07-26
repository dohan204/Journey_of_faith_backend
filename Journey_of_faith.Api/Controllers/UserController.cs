using System.Net.Mime;
using Journey_of_faith.Api.dtos;
using Journey_of_faith.Application.common.dtos;
using Journey_of_faith.Application.usecases.users.commands;
using Journey_of_faith.Application.usecases.users.queries;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                message = "đăng ký thành công",
                success = true
            });
        }
        [HttpGet]
        // [Consumes(MediaTypeNames.Application.j)]
        // [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        
        public async Task<IActionResult> GetUsers([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search)
        {
            var result = await _mediator.Send(new GetUsersQuery { Page = page, PageSize = pageSize, Search = search});
            return Ok(new ApiResponse<PagedResult<UserResponseDto>>
            {
                Data = result,
                Message = "Lấy dữ liệu thành công."
            });
        }
        [HttpDelete("{UsreId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string UsreId)
        {
            var isDeleted = await _mediator.Send(new DeleteUserCommand{Id = UsreId});

            return Ok(isDeleted);
        }
    }
}
