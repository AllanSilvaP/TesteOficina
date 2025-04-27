using Xunit;
using Moq;
using FluentAssertions;
using RO.DevTest.WebApi.Controllers;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Tests.Controllers
{
    public class SalesControllerTests
    {
        private readonly SalesController _controller;
        private readonly Mock<ISaleRepository> _repositoryMock;

        public SalesControllerTests()
        {
            _repositoryMock = new Mock<ISaleRepository>();
            _controller = new SalesController(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnResult()
        {
            var result = await _controller.Get();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ShouldReturnResult()
        {
            var result = await _controller.Create(new());
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_ShouldReturnResult()
        {
            var result = await _controller.Update(Guid.NewGuid(), new());
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_ShouldReturnResult()
        {
            var result = await _controller.Delete(Guid.NewGuid());
            result.Should().NotBeNull();
        }
    }
}
