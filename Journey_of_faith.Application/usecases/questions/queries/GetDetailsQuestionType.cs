using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetDetailsQuestionTypeQuery :  IRequest<QuestionType?>
    {
        public int Id { get; set; }
    }

    public class GetDetailsQuestionTypeHandler : IRequestHandler<GetDetailsQuestionTypeQuery, QuestionType?>
    {
        private readonly IQuestionRepository questionRepository;
        public GetDetailsQuestionTypeHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository; 
        }

        public async Task<QuestionType?> Handle(GetDetailsQuestionTypeQuery request, CancellationToken token)
        {
            return await questionRepository.GetDetailsQuestionType(request.Id);
        }
    }
}
