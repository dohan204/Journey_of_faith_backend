using Journey_of_faith.Application.usecases.quizs.commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
        {
            var quiz = await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                Message = "Tạo đề thi thành công",
                Data = quiz
            });
        }
    }
}
