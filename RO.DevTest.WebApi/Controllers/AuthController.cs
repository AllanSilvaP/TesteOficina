using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using RO.DevTest.Application.Features.Auth.Commands.LoginCommand;
using RO.DevTest.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Namotion.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;


namespace RO.DevTest.WebApi.Controllers;

[Route("api/auth")]
[OpenApiTags("Auth")]

//LOGIN HANDLER HERE
public class AuthController(IMediator mediator, AppDbContext context, IConfiguration configuration) : Controller
{
    private readonly IMediator _mediator = mediator;
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);

        if (customer == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var hasher = new PasswordHasher<Customer>();
        var result = hasher.VerifyHashedPassword(customer, customer.PasswordHash, request.Password);

        if (result != PasswordVerificationResult.Success)
        {
            return Unauthorized("Invalid email or password");
        }

        var token = GenerateJwtToken(customer);

        customer.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    //PASSWORD TOKEN

    private string GenerateJwtToken(Customer customer)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, customer.Name),
            new Claim(ClaimTypes.Role, customer.Role ?? "Customer")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

}
