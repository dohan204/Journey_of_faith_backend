using Journey_of_faith.Application.usecases.quizs.queries;
using Journey_of_faith.Domain.interfaces;
using Moq;

namespace UnitTesting.HandlerTests.quiz
{
    public class GetAllQuizzesHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnAllQuizzesFromRepository()
        {
            var quizzes = new List<QuizView>
            {
                new()
                {
                    Id = 2,
                    Title = "Đề thi 2",
                    Questions = new List<QuestionQuiz>
                    {
                        new()
                        {
                            Id = 10,
                            QuestionContent = "Câu hỏi 1",
                            Ansewrs = new List<AnsewrQuestion>
                            {
                                new() { Id = 100, QuestionId = 10, Content = "Đáp án 1" }
                            }
                        }
                    }
                },
                new() { Id = 1, Title = "Đề thi 1" }
            };
            var repository = new Mock<IExamRepository>();
            repository.Setup(x => x.GetAllQuizzesAsync()).ReturnsAsync(quizzes);
            var handler = new GetAllQuizzesHandler(repository.Object);

            var result = await handler.Handle(new GetAllQuizzesQuery(), CancellationToken.None);

            Assert.Equal(quizzes, result);
            repository.Verify(x => x.GetAllQuizzesAsync(), Times.Once);
        }
    }
}
