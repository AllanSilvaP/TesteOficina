using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SalesController : ControllerBase {
    private readonly ISaleRepository _repository;

    public SalesController(ISaleRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await _repository.GetByIdAsync(id);
        return sale is null ? NotFound() : Ok(sale);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Sale sale)
    {
        await _repository.AddAsync(sale);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Sale sale)
    {
        if (id != sale.Id) return BadRequest();
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _repository.UpdateAsync(sale);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}