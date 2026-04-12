using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetDetailsQuestionLevelCommand : IRequest<QuizLevel?>
    {
        public int Id { get; set; }
    }

    public class GetDetailsQuestionLevelHandler : IRequestHandler<GetDetailsQuestionLevelCommand, QuizLevel?>
    {
        private readonly IQuestionRepository questionRepository;
        public GetDetailsQuestionLevelHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<QuizLevel?> Handle(GetDetailsQuestionLevelCommand request, CancellationToken cancellationToken)
        {
            return await questionRepository.GetDetailQuizLevel(request.Id);
        }
    }
}
