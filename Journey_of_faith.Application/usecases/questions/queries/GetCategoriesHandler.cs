using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<QuestionCategory>>
    {
        private readonly IQuestionRepository questionRepository;
        public GetCategoriesHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionCategory>> Handle(GetCategoriesQuery query, CancellationToken token)
        {
            return await questionRepository.GetAllCategoryQuestion();
        }
    }
}
