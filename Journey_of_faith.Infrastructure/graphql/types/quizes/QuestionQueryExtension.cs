using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.quizes
{
    [ExtendObjectType(typeof(Quiz))]
    public static partial class QuestionQueryExtension
    {
        public static async Task<Question[]> GetQuestionsAsync(
            [Parent] Quiz quiz,
            IQuestionByQuizDataLoader questionByQuizDataLoader,
            CancellationToken cancellationToken)
        {
            return await questionByQuizDataLoader.LoadAsync(quiz.Id, cancellationToken);
        }
    }
}
