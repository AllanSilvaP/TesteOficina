using Xunit;
using Moq;
using FluentAssertions;
using RO.DevTest.WebApi.Controllers;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IProductRepository> _repositoryMock;

        public ProductsControllerTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _controller = new ProductsController(_repositoryMock.Object);
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