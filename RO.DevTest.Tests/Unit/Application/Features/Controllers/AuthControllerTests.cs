using Xunit;
using Moq;
using FluentAssertions;
using RO.DevTest.WebApi.Controllers;
using MediatR;
using Microsoft.Extensions.Configuration;
using RO.DevTest.Persistence;

namespace RO.DevTest.WebApi.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _dbContextMock = new Mock<AppDbContext>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new AuthController(_mediatorMock.Object, _dbContextMock.Object, _configurationMock.Object);
        }

        [Fact]
        public void Login_ShouldReturnResult()
        {
            var request = new AuthController.LoginRequest();
            var result = _controller.Login("user", request);
            result.Should().NotBeNull();
        }
    }
}