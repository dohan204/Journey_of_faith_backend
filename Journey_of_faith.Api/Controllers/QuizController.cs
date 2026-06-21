using Journey_of_faith.Api.dtos;
using Journey_of_faith.Application.usecases.quizs.commands;
using Journey_of_faith.Application.usecases.quizs.queries;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;
        public QuizController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizCommand command)
        {
            var quiz = await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                Message = "Tạo đề thi thành công",
                Data = quiz
            });
        }

        [HttpGet("{id}/details")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetails(int id)
        {
            var details = await _mediator.Send(new GetDetailsQuizQuery { Id = id });
            if(details is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Message = "Không có dữ liệu",
                    Data = details
                });
            }

            return Ok(new ApiResponse<QuizView>
            {
                Message = "Lấy dữ liệu thành công",
                Data = details
            });
        }

        [HttpPost("/submit")]
        //[Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> SubmitExam(SubmitExamCommand command)
        {
            var com = await _mediator.Send(command);
            return Ok(new ApiResponse<SubmitResult>
            {
                Message = "Nọp bài thành công.",
                Data = com
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            await _mediator.Send(new DeleteQuizCommand { Id = id });
            return NoContent();
        }


        [HttpPost("topics")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTopicAsync([FromBody] CreateTopicCommand command)
        {
            await _mediator.Send(command);
            return StatusCode(statusCode: StatusCodes.Status201Created, new
            {
                Message = "Tạo mới chủ đề thành công."
            });
        }
        [HttpDelete("topics")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTopic([FromBody] int id)
        {
            await _mediator.Send(new DeleteTopicCommand { Id = id });
            return NoContent();
        }
    }
}
