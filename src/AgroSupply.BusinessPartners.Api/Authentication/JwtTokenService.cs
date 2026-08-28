using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AgroSupply.BusinessPartners.Api.Authentication;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(
        string subject,
        string role)
    {
        var issuer =
            _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "A configuração Jwt:Issuer não foi encontrada.");

        var audience =
            _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "A configuração Jwt:Audience não foi encontrada.");

        var key =
            _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "A configuração Jwt:Key não foi encontrada.");

        var expirationMinutes =
            _configuration.GetValue<int>(
                "Jwt:ExpirationMinutes");

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                subject),

            new Claim(
                ClaimTypes.Role,
                role),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    expirationMinutes),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}