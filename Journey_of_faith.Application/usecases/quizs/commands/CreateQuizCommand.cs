using FluentValidation;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.quizs.commands
{
    public class CreateQuizCommand : IRequest<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set;  }
        public bool IsDaily { get; set; } = false;
        public int QuestionCount { get; set; }
    }


    public class CreateQuizQuestionCommand
    {
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public int OrderIndex { get; set;  }
    }


    public class CreateQuizValidator : AbstractValidator<CreateQuizCommand>
    {
        public CreateQuizValidator()
        {
            RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Tên đề thi không được bỏ trống")
                .MaximumLength(200).WithMessage("Tên đề thi không được vượt quá 200 ký tự");

            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("Mô tả không được bỏ trống");

            RuleFor(e => e.TimeLimit)
                .NotEmpty().WithMessage("Thời gian làm bài không được bỏ trống")
                .GreaterThan(0).WithMessage("Thời gian làm bài không được nhỏ hơn 0");
        }
    }

    public class CreateQuizQuestionValidator : AbstractValidator<CreateQuizQuestionCommand>
    {
        private readonly IQuestionRepository questionRepository;
        public CreateQuizQuestionValidator(IQuestionRepository questionRepository)
        {

            this.questionRepository = questionRepository;
            RuleFor(e => e.QuizId).NotEmpty().WithMessage("Mã đề kh đucợ để trống");
            RuleFor(e => e.QuestionId).NotEmpty().WithMessage("questionId không được để trống")
                .MustAsync(async (id, cancellationToken) =>
            {
                return await questionRepository.CheckValidId(id, "Question");
            }).WithMessage("Question ID Không hợp lệ");
        }
    }


}
