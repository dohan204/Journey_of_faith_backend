using Azure.Core;
using Journey_of_faith.Api.dtos;
using Journey_of_faith.Application.usecases.auth.commands;
using Journey_of_faith.Application.usecases.auth.queries;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _me;
        public AuthController(IMediator me)
        {
            _me = me;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginQuery query)
        {
            var login = await _me.Send(query);
            return Ok(login);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] CreateRefreshTokenCommand command)
        {
            var refresh = await _me.Send(command);
            return Ok(refresh);
        }
        [HttpPatch("change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            await _me.Send(command);
            return NoContent();
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? search)
        {
            var roles = await _me.Send(new GetRolesQuery {Page = page,PageSize = pageSize,Search = search});
            return Ok(new ApiResponse<PagedResult<Role>>
            {
                Data = roles,
                Message = "Lấy dữ liệu thành công"
            });
        }
        [HttpGet("roles/users-role")]
        public async Task<IActionResult> GetTotalRole()
        {
            var total = await _me.Send(new GetTotalUserPerRoleQuery());
            return Ok(new ApiResponse<Dictionary<string, int>>
            {
                Message = "Lấy dữ liệu thành công",
                Data = total
            });
        }
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var roleId = await _me.Send(command);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<string>
            {
                Message = "Tạo vai trò thành công.",
                Data = roleId
            });
        }

        [HttpPost("roles/add-permission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionCommand command)
        {
            var isSuccess = await _me.Send(command);
            return Ok(new ApiResponse<bool>
            {
                Message = "Thêm vai trò thành công",
                Data = isSuccess
            });
        }

        [HttpGet("roles/get-permissions")]
        public async Task<IActionResult> GetPermission()
        {
            var result = await _me.Send(new GetPermissionsQuery());
            return Ok(new ApiResponse<List<object>>
            {
                Message = "Lay quuyen thanfh cong",
                Data = result
            });
        }
        [HttpDelete("roles/{Name}")]
        public async Task<IActionResult> Delete([FromRoute] string Name)
        {
            await _me.Send(new DeleteRoleCommand { RoleName = Name});
            return NoContent();
        }

        [HttpDelete("roles/remove-user")]
        public async Task<IActionResult> RemoveUserRole([FromBody] DeleteUserFromRoleCommand command)
        {
            await _me.Send(new DeleteUserFromRoleCommand {userId = command.userId, RoleName = command.RoleName} );
            return NoContent();
        }
        [HttpPut("roles")]
        public async Task<IActionResult> UpdateRole([FromBody] UpDateRoleCommand command)
        {
            await _me.Send(command);
            return NoContent();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _me.Send(command);
            return NoContent();
        } 

    }
}
