// File: Controllers/SalesController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RO.DevTest.Application.Interfaces;
using RO.DevTest.Domain.Entities;
using System.Security.Claims;

namespace RO.DevTest.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleRepository _repository;

    public SalesController(ISaleRepository repository) => _repository = repository;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (IsAdmin())
        {
            return Ok(await _repository.GetAllAsync());
        }

        var customerId = GetUserId();
        var allSales = await _repository.GetAllAsync();
        var customerSales = allSales.Where(s => s.CustomerId.ToString() == customerId);

        return Ok(customerSales);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await _repository.GetByIdAsync(id);
        if (sale is null) return NotFound();

        if (!IsAdmin() && sale.CustomerId.ToString() != GetUserId())
        {
            return Forbid();
        }

        return Ok(sale);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Sale sale)
    {
        await _repository.AddAsync(sale);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Sale sale)
    {
        if (id != sale.Id) return BadRequest();
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        await _repository.UpdateAsync(sale);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    // --- Private helpers ---

    private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    private string? GetUserRole() => User.FindFirst(ClaimTypes.Role)?.Value;

    private bool IsAdmin() => GetUserRole() == "Admin";
}
