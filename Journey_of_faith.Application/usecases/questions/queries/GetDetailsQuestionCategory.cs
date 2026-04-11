using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetDetailsQuestionCategoryQuery : IRequest<QuestionCategory?>
    {
        public int Id { get; set; }
    }


    public class GetDetailsQuestionCategoryHandler : IRequestHandler<GetDetailsQuestionCategoryQuery, QuestionCategory?>
    {
        private readonly IQuestionRepository questionRepository;
        public GetDetailsQuestionCategoryHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<QuestionCategory?> Handle(GetDetailsQuestionCategoryQuery request, CancellationToken token)
        {
            return await questionRepository.GetDetailsQuestionCategory(request.Id);
        }
    }
}
