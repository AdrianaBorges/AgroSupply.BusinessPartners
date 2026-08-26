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

    [Fact]
    public void Update_ShouldChangeBusinessPartnerData_WhenDataIsValid()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        const string newName = "Agro Forte Distribuidora Ltda";
        const string newCpf = "98765432100";
        var newBirthDate = new DateTime(1985, 10, 20);

        // Act
        businessPartner.Update(
            newName,
            newCpf,
            newBirthDate);

        // Assert
        Assert.Equal(newName, businessPartner.Name);
        Assert.Equal(newCpf, businessPartner.Cpf);
        Assert.Equal(newBirthDate, businessPartner.BirthDate);
    }

    [Fact]
    public void Update_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            businessPartner.Update(
                "",
                "98765432100",
                new DateTime(1985, 10, 20)));

        // Assert
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Update_ShouldThrowArgumentException_WhenCpfIsEmpty()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            businessPartner.Update(
                "Agro Forte Distribuidora Ltda",
                "",
                new DateTime(1985, 10, 20)));

        // Assert
        Assert.Equal("cpf", exception.ParamName);
    }
}