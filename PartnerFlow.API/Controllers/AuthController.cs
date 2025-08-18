using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PartnerFlow.Domain.DTOs.Auth;
using PartnerFlow.Infrastructure.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PartnerFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly AuthSettings _authSettings;

    public AuthController(
        IOptions<JwtSettings> jwtOptions,
        IOptions<AuthSettings> authOptions)
    {
        _jwtSettings = jwtOptions.Value;
        _authSettings = authOptions.Value;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Usuario != _authSettings.Usuario || request.Senha != _authSettings.Senha)
            return Unauthorized(new { erro = "Usuário ou senha inválidos." });

        var token = GerarTokenJwt(request.Usuario);
        return Ok(new { token });
    }

    private string GerarTokenJwt(string usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}