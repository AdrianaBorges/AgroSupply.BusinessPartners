using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AgroSupply.BusinessPartners.Api.Tests.Controllers;

public class BusinessPartnersControllerTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BusinessPartnersControllerTests(
        CustomWebApplicationFactory factory)
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

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync(
            "/api/BusinessPartners");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_ShouldReturnOk_WhenBusinessPartnerExists()
    {
        // Arrange - Create
        var createRequest = new
        {
            name = "Agro Update Ltda",
            cpf = "12345678901",
            birthDate = new DateTime(1990, 5, 15)
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/BusinessPartners",
            createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createContent = await createResponse.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(createContent);

        var id = document.RootElement
            .GetProperty("id")
            .GetGuid();

        // Arrange - Update
        var updateRequest = new
        {
            name = "Agro Update Distribuidora Ltda",
            cpf = "98765432100",
            birthDate = new DateTime(1985, 10, 20)
        };

        // Act
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/BusinessPartners/{id}",
            updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var businessPartner =
            await updateResponse.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            id,
            businessPartner.GetProperty("id").GetGuid());

        Assert.Equal(
            updateRequest.name,
            businessPartner.GetProperty("name").GetString());

        Assert.Equal(
            updateRequest.cpf,
            businessPartner.GetProperty("cpf").GetString());

        Assert.Equal(
            updateRequest.birthDate,
            businessPartner.GetProperty("birthDate").GetDateTime());

        Assert.True(
            businessPartner.GetProperty("isActive").GetBoolean());
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenBusinessPartnerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new
        {
            name = "Agro Inexistente Ltda",
            cpf = "12345678901",
            birthDate = new DateTime(1990, 5, 15)
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/BusinessPartners/{id}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenBusinessPartnerExists()
    {
        // Arrange - Create
        var createRequest = new
        {
            name = "Parceiro para Inativacao Ltda",
            cpf = "74185296300",
            birthDate = new DateTime(1992, 4, 12)
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/BusinessPartners",
            createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createContent = await createResponse.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(createContent);

        var id = document.RootElement
            .GetProperty("id")
            .GetGuid();

        // Act
        var deleteResponse = await _client.DeleteAsync(
            $"/api/BusinessPartners/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/BusinessPartners/{id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var businessPartner =
            await getResponse.Content.ReadFromJsonAsync<JsonElement>();

        Assert.False(
            businessPartner.GetProperty("isActive").GetBoolean());

        Assert.NotEqual(
            JsonValueKind.Null,
            businessPartner.GetProperty("deactivatedAt").ValueKind);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenBusinessPartnerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/BusinessPartners/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddPhoneNumber_ShouldReturnOk_WhenBusinessPartnerExists()
    {
        // Arrange - Create Business Partner
        var createRequest = new
        {
            name = "Agro Telefones Ltda",
            cpf = "85274196300",
            birthDate = new DateTime(1991, 6, 20)
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/BusinessPartners",
            createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createContent = await createResponse.Content.ReadAsStringAsync();

        using var document = JsonDocument.Parse(createContent);

        var id = document.RootElement
            .GetProperty("id")
            .GetGuid();

        var phoneRequest = new
        {
            type = 1,
            number = "21999999999"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/BusinessPartners/{id}/phone-numbers",
            phoneRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var businessPartner =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        var phoneNumbers =
            businessPartner.GetProperty("phoneNumbers");

        Assert.Equal(1, phoneNumbers.GetArrayLength());

        var phoneNumber = phoneNumbers[0];

        Assert.Equal(
            phoneRequest.type,
            phoneNumber.GetProperty("type").GetInt32());

        Assert.Equal(
            phoneRequest.number,
            phoneNumber.GetProperty("number").GetString());
    }

    [Fact]
    public async Task AddPhoneNumber_ShouldReturnNotFound_WhenBusinessPartnerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var request = new
        {
            type = 1,
            number = "21999999999"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            $"/api/BusinessPartners/{id}/phone-numbers",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPhoneNumber_ShouldReturnOk_WhenPhoneNumberExists()
    {
        // Arrange
        var (businessPartnerId, phoneNumberId) =
            await CreateBusinessPartnerWithPhoneNumberAsync();

        // Act
        var response = await _client.GetAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var phoneNumber =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            phoneNumberId,
            phoneNumber.GetProperty("id").GetGuid());

        Assert.Equal(
            1,
            phoneNumber.GetProperty("type").GetInt32());

        Assert.Equal(
            "21999999999",
            phoneNumber.GetProperty("number").GetString());
    }

    [Fact]
    public async Task GetPhoneNumber_ShouldReturnNotFound_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartnerId =
            await CreateBusinessPartnerAsync();

        var phoneNumberId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePhoneNumber_ShouldReturnOk_WhenPhoneNumberExists()
    {
        // Arrange
        var (businessPartnerId, phoneNumberId) =
            await CreateBusinessPartnerWithPhoneNumberAsync();

        var request = new
        {
            type = 2,
            number = "2133334444"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var phoneNumber =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.Equal(
            phoneNumberId,
            phoneNumber.GetProperty("id").GetGuid());

        Assert.Equal(
            request.type,
            phoneNumber.GetProperty("type").GetInt32());

        Assert.Equal(
            request.number,
            phoneNumber.GetProperty("number").GetString());
    }

    [Fact]
    public async Task UpdatePhoneNumber_ShouldReturnNotFound_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartnerId =
            await CreateBusinessPartnerAsync();

        var phoneNumberId = Guid.NewGuid();

        var request = new
        {
            type = 2,
            number = "2133334444"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeletePhoneNumber_ShouldReturnNoContent_WhenPhoneNumberExists()
    {
        // Arrange
        var (businessPartnerId, phoneNumberId) =
            await CreateBusinessPartnerWithPhoneNumberAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePhoneNumber_ShouldReturnNotFound_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartnerId =
            await CreateBusinessPartnerAsync();

        var phoneNumberId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers/{phoneNumberId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<Guid> CreateBusinessPartnerAsync()
    {
        var request = new
        {
            name = $"Agro Teste {Guid.NewGuid():N}",
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

    private async Task<(Guid BusinessPartnerId, Guid PhoneNumberId)>
        CreateBusinessPartnerWithPhoneNumberAsync()
    {
        var businessPartnerId =
            await CreateBusinessPartnerAsync();

        var phoneRequest = new
        {
            type = 1,
            number = "21999999999"
        };

        var response = await _client.PostAsJsonAsync(
            $"/api/BusinessPartners/{businessPartnerId}/phone-numbers",
            phoneRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var businessPartner =
            await response.Content.ReadFromJsonAsync<JsonElement>();

        var phoneNumberId = businessPartner
            .GetProperty("phoneNumbers")[0]
            .GetProperty("id")
            .GetGuid();

        return (
            businessPartnerId,
            phoneNumberId);
    }
}