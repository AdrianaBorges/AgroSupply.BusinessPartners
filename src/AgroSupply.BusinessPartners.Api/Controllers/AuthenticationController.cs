using AgroSupply.BusinessPartners.Api.Authentication;
using AgroSupply.BusinessPartners.Api.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AgroSupply.BusinessPartners.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IAuthenticationService authenticationService,
        IConfiguration configuration,
        ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT access token.
    /// </summary>
    /// <param name="request">Authentication credentials.</param>
    /// <returns>The JWT access token when authentication succeeds.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(
        LoginRequest request)
    {
        var token =
            _authenticationService.Authenticate(
                request.Username,
                request.Password);

        if (token is null)
        {
            _logger.LogWarning(
                "Tentativa de autenticação inválida para o usuário {Username}.",
                request.Username);

            return Unauthorized();
        }

        var expirationMinutes =
            _configuration.GetValue<int>(
                "Jwt:ExpirationMinutes");

        _logger.LogInformation(
            "Usuário {Username} autenticado com sucesso.",
            request.Username);

        return Ok(
            new LoginResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresInMinutes = expirationMinutes
            });
    }
}