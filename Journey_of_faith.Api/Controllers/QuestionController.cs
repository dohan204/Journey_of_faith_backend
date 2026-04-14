using Journey_of_faith.Application.usecases.questions.commands;
using Journey_of_faith.Application.usecases.questions.queries;
using Journey_of_faith.Domain.entities.quiz;
using MediatR;
using Journey_of_faith.Api.dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("Journey_of_faith/[controller]")]
    public sealed class QuestionController(IMediator mediator, IFileStorageService service) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IFileStorageService _fileStorageService = service;
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

        [HttpGet("quiz")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLevel()
        {
            var result = await _mediator.Send(new GetLevelQuery());
            if(!result.Any())
            {
                return Ok(new ApiResponse<IEnumerable<QuizLevel>>
                {
                    Message = "Không dó dữ liệu",
                    Data = result
                });
            }
            return Ok(new ApiResponse<IEnumerable<QuizLevel>>
            {
                Message = "Lấy dữ liệu thành công",
                Data = result
            });

        }
        [HttpGet("quiz/{quizId}/details")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailsQuiz([FromRoute] int quizId)
        {
            var quiz = await _mediator.Send(new GetDetailsQuestionLevelCommand { Id = quizId });
            if(quiz is null)
            {
                return NotFound(new ApiResponse<QuizLevel>
                {
                    Message = "Không tìm thấy dữ liệu",
                    Data = quiz
                });
            }

            return Ok(new ApiResponse<QuizLevel>
            {
                Message = "Lấy dữ liệu thành công",
                Data = quiz
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
        [HttpGet("question_type")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllType()
        {
            var types = await _mediator.Send(new GetQuestionTypeQuery());
            if(!types.Any())
            {
                return Ok(new ApiResponse<IEnumerable<QuestionType>>
                {
                    Message = "Không có dữ liệu",
                    Data = types
                });
            }

            return Ok(new ApiResponse<IEnumerable<QuestionType>>
            {
                Message = "Lấy dữ lieuj thành công.",
                Data = types
            });
        }
        [HttpGet("question_type/{id}/details")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailsType([FromRoute] int id)
        {
            var type = await _mediator.Send(new GetDetailsQuestionTypeQuery { Id = id });
            if(type is null)
            {
                return NotFound(new ApiResponse<QuestionType>
                {
                    Message = "Không có dữ liệu",
                    Data = type
                });
            }
            return Ok(new ApiResponse<QuestionType>
            {
                Message = "Lấy dữ liệu thành công",
                Data = type
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
        [HttpGet("question_category")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            if(!categories.Any())
            {
                return Ok(new ApiResponse<IEnumerable<QuestionCategory>>
                {
                    Message = "Không dó dữ liệu",
                    Data = categories
                });
            }
            return Ok(new ApiResponse<IEnumerable<QuestionCategory>>
            {
                Message = "Lấy dữ liệu thành công",
                Data = categories
            });
        }

        [HttpGet("question_category/{id}/details")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailsCategory([FromRoute] int id)
        {
            var category = await _mediator.Send(new GetDetailsQuestionCategoryQuery { Id = id });
            if(category is null)
            {
                return NotFound(new ApiResponse<QuestionCategory>
                {
                    Message = "Không có dữ liệu",
                    Data = category
                });
            }
            return Ok(new ApiResponse<QuestionCategory>
            {
                Message = "Lấy dữ liệu thành công.",
                Data = category
            });
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateQuestion([FromForm] CreateQuestionCommand command, IFormFile? file)
        {
            if(file != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "question");
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var UniquiFile = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(path, UniquiFile);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                command.ImageUrl = Path.Combine("uploads", "question", UniquiFile);
            }
            await _mediator.Send(command);
            return Ok(new ApiResponse<object>
            {
                Message = "Tạo Câu hỏi thành công",
                Data = command
            });
        }
        [HttpGet("{Id}/details")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailsQuestion(int Id)
        {
            var data = await _mediator.Send(new GetDetailsQuestionQuery  { Id = Id });
            if(data is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Message = "Không tìm thấy dữ liệu",
                    Data = data,
                });
            }

            return Ok(new ApiResponse<QuestionView>
            {
                Message = "Lấy dữ liệu thành công.",
                Data = data,
            });
        }

        [HttpPut]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{Id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteQuestion(int Id)
        {
            await _mediator.Send(new DeleteQuestionCommand { Id = Id });
            return NoContent();
        }
    }
}



public static class StartNameApi
{
    public const string name = "Journey_of_faith";
}