using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.types.quizes
{
    [ExtendObjectType(typeof(Topic))]
    public static partial class QuizQueryExtension
    {
        public static async Task<Quiz[]> Quizs(
            [Parent] Topic topic,
            IQuizByTopicDataLoader quizByTopicDataLoader,
            CancellationToken cancellation)
        {
            return await quizByTopicDataLoader.LoadAsync(topic.Id, cancellation);
        }
    }
}
