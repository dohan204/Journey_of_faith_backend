using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetCountQuestionQuery : IRequest<int>
    {
    }

    public class GetCountQuestionHandler : IRequestHandler<GetCountQuestionQuery, int>
    {
        private readonly IQuestionRepository questionRepository;
        public GetCountQuestionHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<int> Handle(GetCountQuestionQuery query, CancellationToken token)
        {
            return await questionRepository.GetCountQuestion();
        }
    }
}
