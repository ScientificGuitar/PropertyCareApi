using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyCare.Application.Dtos;
using PropertyCare.Domain.Entities;
using PropertyCare.Infrastructure;

namespace PropertyCare.Web.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(PropertyCareDbContext db, IConfiguration config) : ControllerBase
{
    private readonly PropertyCareDbContext _db = db;
    private readonly IConfiguration _config = config;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto, CancellationToken cancellationToken)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
            return BadRequest("Email already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User(dto.Email, passwordHash, UserRole.Tenant);
        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);

        if (user is null)
            return Unauthorized();

        var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!valid)
            return Unauthorized();

        var token = GenerateJwt(user);

        return Ok(new { token });
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
