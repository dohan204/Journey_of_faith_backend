using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetLevelHandler : IRequestHandler<GetLevelQuery, IEnumerable<QuizLevel>> 
    {
        private readonly IQuestionRepository questionRepository;
        public GetLevelHandler(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuizLevel>> Handle(GetLevelQuery query, CancellationToken token)
        {
            var levels = await questionRepository.GetLevelsAsync();
            return levels;
        }
    }
}
