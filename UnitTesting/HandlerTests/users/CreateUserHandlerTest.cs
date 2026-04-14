using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Application.usecases.users.commands;
using Journey_of_faith.Application.usecases.users.validations;
using Journey_of_faith.Domain.entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTesting.HandlerTests.users
{
    public class CreateUserHandlerTest
    {
        private readonly CreateUserHandler _handler;
        private readonly Mock<IIdentityService> _service;
        public CreateUserHandlerTest()
        {
            _service = new Mock<IIdentityService>();
            _handler = new CreateUserHandler(_service.Object);
        }


        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenInputValid()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Username = "testuser",
                Password = "Test@1234",
                Name = "Test User",
                Email = "dohan@gmail.com"
            };

            _service.Setup(s => s.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(true);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);

            _service.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldCreateUser_WhenEmailNotExists()
        {
            // arrange 
            _service.Setup(s => s.ExistsEmail(It.IsAny<string>()))
                .ReturnsAsync(false);


            _service.Setup(e => e.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(true);


            var command = new CreateUserCommand
            {
                Username = "testuser",
                Password = "Test@1234",
                Name = "Test User",
                Email = "dohan@gmail.com"
            };

            // act
            var result = await _handler.Handle(command, CancellationToken.None);

            // assert
            Assert.True(result);

            _service.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Once);
        }
        [Fact]
        public async Task Handle_ShouldThrowConflictException_WhenEmailExists()
        {
            // arrange
            _service.Setup(s => s.ExistsEmail("Test@gmail.com"))
                 .ReturnsAsync(true);


            var command = new CreateUserCommand
            {
                Username = "testuser",
                Password = "Test@1234",
                Name = "Test User",
                Email = "Test@gmail.com"
            };

            // act & assert
            await Assert.ThrowsAsync<ConfictException>(
                () => _handler.Handle(command, CancellationToken.None));

            _service.Verify(s => s.CreateAsync(It.IsAny<User>()), Times.Never);
        }


    }
}
