using Journey_of_faith.Api.dtos;
using Journey_of_faith.Application.usecases.churchs.commands;
using Journey_of_faith.Application.usecases.churchs.dtos;
using Journey_of_faith.Application.usecases.churchs.queries;
using Journey_of_faith.Domain.entities.catholic;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChurchesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChurchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChurch([FromBody] CreateChurchCommand command, IFormFile? file)
        {
            if (file != null)
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "uploads", "churchs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var uniqueFile = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
                var fullPath = System.IO.Path.Combine(path, uniqueFile);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                command.Thumbnail = System.IO.Path.Combine("uploads", "churchs", uniqueFile);
            }

            var churchId = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<int>
            {
                Message = "Tạo nhà thờ thành công.",
                Data = churchId
            });
        }

        [HttpPost("dioceses")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDiocese([FromForm] CreateDioceseCommand command, IFormFile? file)
        {
            if (file != null)
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "uploads", "dioceses");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var uniqueFile = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(file.FileName)}";
                var fullPath = System.IO.Path.Combine(path, uniqueFile);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                command.Thumbnail = System.IO.Path.Combine("uploads", "dioceses", uniqueFile);
            }

            var dioceseId = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<int>
            {
                Message = "Tạo giáo phận thành công.",
                Data = dioceseId
            });
        }
        [HttpGet("dioceses")]
        public async Task<IActionResult> GetDiocese() {
            var result = await _mediator.Send(new GetDioceseQuery());
            return Ok(new ApiResponse<IEnumerable<Diocese>> {
                Message = "Lấy dữ liệu thành công.",
                Data = result
            });
        }
        [HttpGet("search")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchChurches([FromQuery] SearchChurchQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<IEnumerable<ChurchListItemView>>
            {
                Message = result.Any() ? "Lấy danh sách nhà thờ thành công." : "Không có nhà thờ phù hợp.",
                Data = result
            });
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChurchDetails([FromRoute] int id)
        {
            var church = await _mediator.Send(new GetChurchDetailsQuery { Id = id });
            if (church is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Message = "Không tìm thấy nhà thờ.",
                    Data = new { Id = id }
                });
            }

            return Ok(new ApiResponse<Church>
            {
                Message = "Lấy chi tiết nhà thờ thành công.",
                Data = church
            });
        }

        [HttpPost("{churchId:int}/follow")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> FollowChurch([FromRoute] int churchId)
        {
            await _mediator.Send(new FollowChurchCommand { ChurchId = churchId });
            return Ok(new ApiResponse<object>
            {
                Message = "Đã thêm nhà thờ vào danh sách theo dõi.",
                Data = new { ChurchId = churchId }
            });
        }

        [HttpDelete("{churchId:int}/follow")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> UnfollowChurch([FromRoute] int churchId)
        {
            await _mediator.Send(new UnfollowChurchCommand { ChurchId = churchId });
            return Ok(new ApiResponse<object>
            {
                Message = "Đã hủy theo dõi nhà thờ.",
                Data = new { ChurchId = churchId }
            });
        }

        [HttpGet("following")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowingChurches()
        {
            var result = await _mediator.Send(new GetFollowedChurchesQuery());
            return Ok(new ApiResponse<IEnumerable<ChurchListItemView>>
            {
                Message = result.Any()
                    ? "Lấy danh sách nhà thờ theo dõi thành công."
                    : "Bạn chưa theo dõi nhà thờ nào.",
                Data = result
            });
        }

        [HttpGet("mass-schedules/personalized")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPersonalizedMassSchedules([FromQuery] GetPersonalizedMassSchedulesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<IEnumerable<PersonalizedMassScheduleItemDto>>
            {
                Message = result.Any()
                    ? "Lấy lịch lễ cá nhân hóa thành công."
                    : "Không có lịch lễ trong phạm vi lọc hoặc bạn chưa theo dõi nhà thờ nào.",
                Data = result
            });
        }

        [HttpGet("reminder-setting")]
        [Authorize]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReminderSetting()
        {
            var result = await _mediator.Send(new GetMassReminderSettingQuery());
            return Ok(new ApiResponse<ReminderSettingView>
            {
                Message = "Lấy cấu hình nhắc lễ thành công.",
                Data = result
            });
        }

        [HttpPut("reminder-setting")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateReminderSetting([FromBody] UpdateMassReminderSettingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<ReminderSettingView>
            {
                Message = "Cập nhật cấu hình nhắc lễ thành công.",
                Data = result
            });
        }


        [HttpPost("dailyWords")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDailyWord([FromBody] CreateDailyWordCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "Create Success."
            });
        }
        [HttpGet("dailyWords/search")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDailyWord([FromQuery] GetDailyWordCommand command)
        {
            var daily = await _mediator.Send(command);

            if(daily is null) 
                return Ok(new
                {
                    Message = "Not found.",
                    Data = ""
                });
            return Ok(new ApiResponse<DailyWord>
            {
                Message = "Get success.",
                Data = daily
            });
        }
    }
}
