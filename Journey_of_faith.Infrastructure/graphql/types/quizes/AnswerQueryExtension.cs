using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.quizes
{
    [ExtendObjectType(typeof(Question))]
    public static partial class AnswerQueryExtension
    {
        public static async Task<Answer[]> GetAnswersAsync([Parent] Question question, 
            IAnswerByQuestionDataLoader answerByQuestionDataLoader, CancellationToken cancellation)
        {
            return await answerByQuestionDataLoader.LoadAsync(question.Id, cancellation);
        }
    }
}
