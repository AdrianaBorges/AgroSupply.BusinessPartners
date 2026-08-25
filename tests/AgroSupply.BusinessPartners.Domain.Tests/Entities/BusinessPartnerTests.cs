using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Domain.Tests.Entities;

public class BusinessPartnerTests
{
    [Fact]
    public void Constructor_ShouldCreateBusinessPartner_WhenNameIsValid()
    {
        // Arrange
        const string name = "João da Silva";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        var businessPartner = new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.Equal(name, businessPartner.Name);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        const string name = "";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        Action action = () => new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsWhiteSpace()
    {
        // Arrange
        const string name = "   ";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        Action action = () => new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_ShouldGenerateId_WhenBusinessPartnerIsCreated()
    {
        // Arrange
        const string name = "João da Silva";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        var businessPartner = new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.NotEqual(Guid.Empty, businessPartner.Id);
    }

    [Fact]
    public void Constructor_ShouldCreateBusinessPartner_WithValidData()
    {
        // Arrange
        const string name = "João da Silva";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        var businessPartner = new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.Equal(name, businessPartner.Name);
        Assert.Equal(cpf, businessPartner.Cpf);
        Assert.Equal(birthDate, businessPartner.BirthDate);
        Assert.True(businessPartner.IsActive);
    }
}