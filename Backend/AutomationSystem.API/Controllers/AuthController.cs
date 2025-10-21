using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutomationSystem.Infrastructure.Data;
using AutomationSystem.Core.Models;

namespace AutomationSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: user not found - {Email}", request.Email);
                return Unauthorized(new { message = "Identifiants incorrects" });
            }

            // Pour la démo, on accepte le mot de passe simple
            // En production, utiliser BCrypt.Net pour vérifier le hash
            if (user.PasswordHash != request.Password)
            {
                _logger.LogWarning("Login attempt failed: incorrect password - {Email}", request.Email);
                return Unauthorized(new { message = "Identifiants incorrects" });
            }

            // Mettre à jour la dernière connexion
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Générer le token JWT
            var token = GenerateJwtToken(user);

            _logger.LogInformation("User logged in successfully - {Email}", request.Email);

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return StatusCode(500, new { message = "Erreur lors de la connexion" });
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? "YourSecretKeyHere_ChangeInProduction_MustBe32CharsMinimum";
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "AutomationSystem";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "AutomationSystemClient";
        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

