using Journey_of_faith.Application.usecases.questions.commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    public sealed class QuestionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("quiz")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Quiz([FromBody] CreateQuizLevelCommand command)
        {
            var status = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "Tạo thành công",
                Status = status
            });
        }
        [HttpPost("question_type")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> QuestionType([FromBody] CreateQuestionTypeCommand command)
        {
            var status = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "Tạo thành công",
                Status = status
            });
        }
        [HttpPost("question_category")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> QuestionCategory([FromBody] CreateQuestionCategoryCommand command)
        {
            var status = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, new
            {
                Message = "Tạo thành công",
                Status = status
            });
        }
    }
}



public static class StartNameApi
{
    public const string name = "Journey_of_faith";
}