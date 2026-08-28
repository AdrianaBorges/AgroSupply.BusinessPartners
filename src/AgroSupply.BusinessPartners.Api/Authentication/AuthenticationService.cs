namespace AgroSupply.BusinessPartners.Api.Authentication;

public class AuthenticationService
    : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly JwtTokenService _jwtTokenService;

    public AuthenticationService(
        IConfiguration configuration,
        JwtTokenService jwtTokenService)
    {
        _configuration = configuration;
        _jwtTokenService = jwtTokenService;
    }

    public string? Authenticate(
        string username,
        string password)
    {
        var configuredUsername =
            _configuration["Authentication:Username"];

        var configuredPassword =
            _configuration["Authentication:Password"];

        var configuredRole =
            _configuration["Authentication:Role"];

        if (string.IsNullOrWhiteSpace(configuredUsername) ||
            string.IsNullOrWhiteSpace(configuredPassword) ||
            string.IsNullOrWhiteSpace(configuredRole))
        {
            throw new InvalidOperationException(
                "As configurações de autenticação não foram encontradas.");
        }

        if (!string.Equals(
                username,
                configuredUsername,
                StringComparison.Ordinal) ||
            !string.Equals(
                password,
                configuredPassword,
                StringComparison.Ordinal))
        {
            return null;
        }

        return _jwtTokenService.GenerateToken(
            configuredUsername,
            configuredRole);
    }
}