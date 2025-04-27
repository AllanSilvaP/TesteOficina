using Xunit;
using Moq;
using FluentAssertions;
using RO.DevTest.WebApi.Controllers;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Tests.Controllers
{
    public class SaleItemsControllerTests
    {
        private readonly SaleItemsController _controller;
        private readonly Mock<ISaleItemRepository> _repositoryMock;

        public SaleItemsControllerTests()
        {
            _repositoryMock = new Mock<ISaleItemRepository>();
            _controller = new SaleItemsController(_repositoryMock.Object);
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
