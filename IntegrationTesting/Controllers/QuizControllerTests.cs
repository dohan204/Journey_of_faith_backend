using FluentAssertions;
using Journey_of_faith.Application.usecases.quizs.commands;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting.Controllers
{
    public class QuizControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public QuizControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateTopic_ShouldReturnBadRequest_WhenTopicNameIsEmpty()
        {
            // Arrange
            var command = new CreateTopicCommand
            {
                TopicName = "",
                QuizCount = 0
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Quiz/topics", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateQuiz_ShouldReturnBadRequest_WhenTitleIsEmpty()
        {
            // Arrange
            var command = new CreateQuizCommand
            {
                Title = "",
                Description = "Test Quiz",
                QuestionCount = 0
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Quiz", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
