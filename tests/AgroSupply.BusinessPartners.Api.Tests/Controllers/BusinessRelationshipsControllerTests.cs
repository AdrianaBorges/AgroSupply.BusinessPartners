using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AgroSupply.BusinessPartners.Api.Tests.Controllers;

public class BusinessRelationshipsControllerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BusinessRelationshipsControllerTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WhenBusinessPartnersExist()
    {
        // Arrange
        var supplierId = await CreateBusinessPartnerAsync();
        var buyerId = await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = buyerId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var relationship =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.NotEqual(
            Guid.Empty,
            relationship.GetProperty("id").GetGuid());

        Assert.Equal(
            supplierId,
            relationship.GetProperty("supplierBusinessPartnerId").GetGuid());

        Assert.Equal(
            buyerId,
            relationship.GetProperty("buyerBusinessPartnerId").GetGuid());

        Assert.Equal(
            1,
            relationship.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenRelationshipExists()
    {
        // Arrange
        var relationshipId =
            await CreateBusinessRelationshipAsync();

        // Act
        var response = await _client.GetAsync(
            $"/api/business-relationships/{relationshipId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var relationship =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            relationshipId,
            relationship.GetProperty("id").GetGuid());
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/business-relationships/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenSupplierIsInactive()
    {
        // Arrange
        var supplierId =
            await CreateBusinessPartnerAsync();

        var buyerId =
            await CreateBusinessPartnerAsync();

        await _client.DeleteAsync(
            $"/api/BusinessPartners/{supplierId}");

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = buyerId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenBuyerIsInactive()
    {
        // Arrange
        var supplierId =
            await CreateBusinessPartnerAsync();

        var buyerId =
            await CreateBusinessPartnerAsync();

        await _client.DeleteAsync(
            $"/api/BusinessPartners/{buyerId}");

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = buyerId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }


    private async Task<Guid> CreateBusinessPartnerAsync()
    {
        var request = new
        {
            name = $"Agro B2B Teste {Guid.NewGuid():N}",
            cpf = "96385274100",
            birthDate = new DateTime(1990, 5, 15)
        };

        var response = await _client.PostAsJsonAsync(
            "/api/BusinessPartners",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(content);

        return document.RootElement
            .GetProperty("id")
            .GetGuid();
    }

    private async Task<Guid> CreateBusinessRelationshipAsync()
    {
        var supplierId = await CreateBusinessPartnerAsync();
        var buyerId = await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = buyerId
        };

        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(content);

        return document.RootElement
            .GetProperty("id")
            .GetGuid();
    }

    [Fact]
    public async Task Deactivate_ShouldReturnNoContent_WhenRelationshipExists()
    {
        // Arrange
        var relationshipId =
            await CreateBusinessRelationshipAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/business-relationships/{relationshipId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/business-relationships/{relationshipId}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var relationship =
            await getResponse.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            2,
            relationship.GetProperty("status").GetInt32());

        Assert.NotEqual(
            JsonValueKind.Null,
            relationship.GetProperty("deactivatedAt").ValueKind);
    }

    [Fact]
    public async Task Deactivate_ShouldReturnNotFound_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/business-relationships/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenSupplierAndBuyerAreTheSame()
    {
        // Arrange
        var businessPartnerId =
            await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = businessPartnerId,
            buyerBusinessPartnerId = businessPartnerId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var content =
            await response.Content.ReadAsStringAsync();

        Assert.Contains(
            "Um parceiro de negócio não pode estabelecer uma relação comercial consigo mesmo.",
            content);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenSupplierDoesNotExist()
    {
        // Arrange
        var buyerId =
            await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = Guid.NewGuid(),
            buyerBusinessPartnerId = buyerId
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnNotFound_WhenBuyerDoesNotExist()
    {
        // Arrange
        var supplierId =
            await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = Guid.NewGuid()
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/business-relationships",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenActiveRelationshipAlreadyExists()
    {
        // Arrange
        var supplierId =
            await CreateBusinessPartnerAsync();

        var buyerId =
            await CreateBusinessPartnerAsync();

        var request = new
        {
            supplierBusinessPartnerId = supplierId,
            buyerBusinessPartnerId = buyerId
        };

        var firstResponse =
            await _client.PostAsJsonAsync(
                "/api/business-relationships",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        // Act
        var secondResponse =
            await _client.PostAsJsonAsync(
                "/api/business-relationships",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
    }


}