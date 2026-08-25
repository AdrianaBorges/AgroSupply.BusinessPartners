using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Domain.Tests.Entities;

public class BusinessPartnerTests
{
    [Fact]
    public void Constructor_ShouldCreateBusinessPartner_WhenNameIsValid()
    {
        // Arrange
        const string name = "Agro Forte Ltda";

        // Act
        var businessPartner = new BusinessPartner(name);

        // Assert
        Assert.Equal(name, businessPartner.Name);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        const string name = "";

        // Act
        Action action = () => new BusinessPartner(name);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNameIsWhiteSpace()
    {
        // Arrange
        const string name = "   ";

        // Act
        Action action = () => new BusinessPartner(name);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_ShouldGenerateId_WhenBusinessPartnerIsCreated()
    {
        // Arrange
        const string name = "Agro Forte Ltda";

        // Act
        var businessPartner = new BusinessPartner(name);

        // Assert
        Assert.NotEqual(Guid.Empty, businessPartner.Id);
    }
}