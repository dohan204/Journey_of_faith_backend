using FluentAssertions;
using Journey_of_faith.Application.usecases.churchs.commands;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTesting.Controllers
{
    public class ChurchesControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ChurchesControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetDiocese_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/Churches/dioceses");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SearchChurches_ShouldReturnSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/Churches/search?Keyword=TanDinh");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateChurch_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange - empty church name and invalid diocese ID
            var command = new CreateChurchCommand
            {
                Name = "",
                DioceseId = 0
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Churches", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
