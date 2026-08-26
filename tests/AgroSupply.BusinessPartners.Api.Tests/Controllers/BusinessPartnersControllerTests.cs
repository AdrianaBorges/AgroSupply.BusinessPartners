using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AgroSupply.BusinessPartners.Api.Tests.Controllers;

public class BusinessPartnersControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BusinessPartnersControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenBusinessPartnerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/BusinessPartners/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateAndGetById_ShouldReturnCreatedBusinessPartner()
    {
        // Arrange
        var request = new
        {
            name = "Vale do Campo Ltda",
            cpf = "45678912300",
            birthDate = new DateTime(1988, 3, 10)
        };

        // Act - Create
        var createResponse = await _client.PostAsJsonAsync(
            "/api/BusinessPartners",
            request);

        // Assert - Create
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var content = await createResponse.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(content);

        var id = document.RootElement
            .GetProperty("id")
            .GetGuid();

        Assert.NotEqual(Guid.Empty, id);

        // Act - Get
        var getResponse = await _client.GetAsync(
            $"/api/BusinessPartners/{id}");

        // Assert - Get
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var businessPartner =
            await getResponse.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            id,
            businessPartner.GetProperty("id").GetGuid());

        Assert.Equal(
            request.name,
            businessPartner.GetProperty("name").GetString());

        Assert.Equal(
            request.cpf,
            businessPartner.GetProperty("cpf").GetString());

        Assert.True(
            businessPartner.GetProperty("isActive").GetBoolean());
    }
}