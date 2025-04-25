using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SaleItemsController : ControllerBase {
    private readonly ISaleItemRepository _repository;

    public SaleItemsController(ISaleItemRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var saleItem = await _repository.GetByIdAsync(id);
        return saleItem is null ? NotFound() : Ok(saleItem);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SaleItem saleItem)
    {
        await _repository.AddAsync(saleItem);
        return CreatedAtAction(nameof(GetById), new { id = saleItem.Id }, saleItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, SaleItem saleItem)
    {
        if (id != saleItem.Id) return BadRequest();
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _repository.UpdateAsync(saleItem);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}