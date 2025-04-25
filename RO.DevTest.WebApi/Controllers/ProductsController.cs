using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController : ControllerBase {
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _repository.AddAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Product product)
    {
        if (id != product.Id) return BadRequest();
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _repository.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}