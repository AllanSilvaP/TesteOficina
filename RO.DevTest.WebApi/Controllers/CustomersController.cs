using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;
using RO.DevTest.Domain.Entities;
using System.Security.Claims;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomersController(ICustomerRepository repository) => _repository = repository;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _repository.GetAllAsync());

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer is null) return NotFound();

        if (!IsAdminOrSelf(customer.Id)) return Forbid();

        return Ok(customer);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        if (!IsAdmin() && GetUserId() != customer.Id.ToString())
        {
            return Forbid();
        }

        var hasher = new PasswordHasher<Customer>();
        customer.PasswordHash = hasher.HashPassword(customer, customer.PasswordHash);
        await _repository.AddAsync(customer);

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();

        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        if (!IsAdminOrSelf(existing.Id)) return Forbid();

        var hasher = new PasswordHasher<Customer>();
        customer.PasswordHash = hasher.HashPassword(customer, customer.PasswordHash);
        await _repository.UpdateAsync(customer);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer is null) return NotFound();

        if (!IsAdminOrSelf(customer.Id)) return Forbid();

        await _repository.DeleteAsync(id);

        return NoContent();
    }

    // --- Private helpers ---

    private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    private string? GetUserRole() => User.FindFirst(ClaimTypes.Role)?.Value;

    private bool IsAdmin() => GetUserRole() == "Admin";

    private bool IsAdminOrSelf(Guid customerId)
    {
        var userId = GetUserId();
        return IsAdmin() || userId == customerId.ToString();
    }
}
