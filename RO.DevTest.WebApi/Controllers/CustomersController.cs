using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        var hasher = new PasswordHasher<Customer>();
        customer.PasswordHash = hasher.HashPassword(customer, customer.PasswordHash);
        await _repository.AddAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _repository.UpdateAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
