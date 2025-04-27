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
    public async Task<IActionResult> Get(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var sales = await _repository.GetAllAsync();

        if (!IsAdmin())
        {
            var customerId = GetUserId();
            sales = sales.Where(s => s.CustomerId.ToString() == customerId).ToList();
        }

        var pagedSales = sales
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(pagedSales);
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

    //DATA

    [Authorize(Roles = "Admin")]
    [HttpGet("analytics")]
    public async Task<IActionResult> GetSalesAnalytics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate) {
        var sales = await _repository.GetAllAsync();

        var filteredSales = sales.Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate);

        var totalSales = filteredSales.Count();
        var totalRevenue = filteredSales.Sum(s => s.TotalAmount);

        var saleItems = filteredSales
        .SelectMany(s => s.saleItems)
        .ToList();

        var productRevenue = saleItems
        .GroupBy(i => new {i.ProductId, i.Product.Name})
        .Select(g => new {
            ProductId = g.Key.Name,
            ProductName = g.Key.Name,
            TotalRevenue = g.Sum(x => x.UnitPrice * x.Quantity)
        }) 
        .ToList();

        var result = new {
            TotalSales = totalSales,
            TotalRevenue = totalRevenue,
            Products = productRevenue
        };

        return Ok(result);
    }

    // --- Private helpers ---

    private string? GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    private string? GetUserRole() => User.FindFirst(ClaimTypes.Role)?.Value;

    private bool IsAdmin() => GetUserRole() == "Admin";
}
