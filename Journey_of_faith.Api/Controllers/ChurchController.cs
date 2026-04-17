using Journey_of_faith.Application.usecases.churchs.commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    public class ChurchController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChurchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/formdata")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChurch([FromForm] CreateChurchCommand command, IFormFile? file)
        {
            if(file != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "churchs");
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var uniquiFile = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var relative = Path.Combine(path, uniquiFile);

                using(var stream = new FileStream(relative, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                command.Thumbnail = Path.Combine("uploads", "churchs", uniquiFile);
            }

            await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                Message = "Tạo Nhà thờ thành công",
                Status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChurchDetails(int Id)
        {
            return Ok();
        }


        [HttpPost("diocese")]
        public async Task<IActionResult> CreateDio([FromForm] CreateDioceseCommand command, IFormFile? file)
        {
            if (file != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "dioceses");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var uniquiFile = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var relative = Path.Combine(path, uniquiFile);

                using (var stream = new FileStream(relative, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                command.Thumbnail = Path.Combine("uploads", "dioceses", uniquiFile);
            }

            await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                Message = "Tạo Thành công",
                Status = true
            });
        }

    }
}
