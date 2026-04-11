using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetQuestionTypeHandler : IRequestHandler<GetQuestionTypeQuery, IEnumerable<QuestionType>> 
    {
        private readonly IQuestionRepository questionRepository;
        public GetQuestionTypeHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionType>> Handle(GetQuestionTypeQuery query, CancellationToken token)
        {
            return await questionRepository.GetAllTypeQuestion();
        }
    }
}
