using System.Net;
using System.Net.Http.Json;
using AgroSupply.BusinessPartners.Api.Contracts.Authentication;

namespace AgroSupply.BusinessPartners.Api.Tests.Controllers;

public class AuthenticationControllerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthenticationControllerTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        var request =
            new LoginRequest
            {
                Username = "admin",
                Password = "AgroSupply@2026!"
            };

        var response =
            await _client.PostAsJsonAsync(
                "/api/authentication/login",
                request);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var content =
            await response.Content
                .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(content);
        Assert.False(
            string.IsNullOrWhiteSpace(
                content.AccessToken));
        Assert.Equal(
            "Bearer",
            content.TokenType);
        Assert.Equal(
            60,
            content.ExpiresInMinutes);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        var request =
            new LoginRequest
            {
                Username = "admin",
                Password = "invalid-password"
            };

        var response =
            await _client.PostAsJsonAsync(
                "/api/authentication/login",
                request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetBusinessPartners_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        await using var factory =
            new UnauthenticatedWebApplicationFactory();

        using var client =
            factory.CreateClient();

        var response =
            await client.GetAsync(
                "/api/BusinessPartners");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}