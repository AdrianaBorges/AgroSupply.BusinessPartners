using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

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

    [Fact]
    public void Constructor_ShouldCreateActiveBusinessPartnerWithoutDeactivationDate()
    {
        // Arrange
        const string name = "Agro Forte Ltda";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        var businessPartner = new BusinessPartner(name, cpf, birthDate);

        // Assert
        Assert.True(businessPartner.IsActive);
        Assert.Null(businessPartner.DeactivatedAt);
    }

    [Fact]
    public void Deactivate_ShouldSetBusinessPartnerAsInactiveAndSetDeactivatedAt()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        businessPartner.Deactivate();

        // Assert
        Assert.False(businessPartner.IsActive);
        Assert.NotNull(businessPartner.DeactivatedAt);
    }

    [Fact]
    public void Deactivate_ShouldKeepOriginalDeactivatedAt_WhenAlreadyInactive()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        businessPartner.Deactivate();

        var firstDeactivatedAt = businessPartner.DeactivatedAt;

        // Act
        businessPartner.Deactivate();

        // Assert
        Assert.Equal(firstDeactivatedAt, businessPartner.DeactivatedAt);
    }

    [Fact]
    public void Constructor_ShouldCreateBusinessPartnerWithoutPhoneNumbers()
    {
        // Arrange & Act
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Assert
        Assert.Empty(businessPartner.PhoneNumbers);
    }

    [Fact]
    public void AddPhoneNumber_ShouldAddPhoneNumberToBusinessPartner()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        businessPartner.AddPhoneNumber(
            PhoneNumberType.Mobile,
            "21999999999");

        // Assert
        Assert.Single(businessPartner.PhoneNumbers);

        var phoneNumber = businessPartner.PhoneNumbers.Single();

        Assert.Equal(PhoneNumberType.Mobile, phoneNumber.Type);
        Assert.Equal("21999999999", phoneNumber.Number);
    }

    [Fact]
    public void AddPhoneNumber_ShouldAllowMultiplePhoneNumbers()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        businessPartner.AddPhoneNumber(
            PhoneNumberType.Mobile,
            "21999999999");

        businessPartner.AddPhoneNumber(
            PhoneNumberType.Commercial,
            "2133334444");

        // Assert
        Assert.Equal(2, businessPartner.PhoneNumbers.Count);

        Assert.Contains(
            businessPartner.PhoneNumbers,
            phone => phone.Type == PhoneNumberType.Mobile &&
                     phone.Number == "21999999999");

        Assert.Contains(
            businessPartner.PhoneNumbers,
            phone => phone.Type == PhoneNumberType.Commercial &&
                     phone.Number == "2133334444");
    }

    [Fact]
    public void GetPhoneNumber_ShouldReturnPhoneNumber_WhenItExists()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        businessPartner.AddPhoneNumber(
            PhoneNumberType.Mobile,
            "21999999999");

        var phoneNumber = businessPartner.PhoneNumbers.Single();

        // Act
        var result = businessPartner.GetPhoneNumber(phoneNumber.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phoneNumber.Id, result.Id);
    }

    [Fact]
    public void UpdatePhoneNumber_ShouldUpdatePhoneNumber_WhenItExists()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        businessPartner.AddPhoneNumber(
            PhoneNumberType.Mobile,
            "21999999999");

        var phoneNumber = businessPartner.PhoneNumbers.Single();

        // Act
        var updated = businessPartner.UpdatePhoneNumber(
            phoneNumber.Id,
            PhoneNumberType.Residential,
            "2133334444");

        // Assert
        Assert.True(updated);

        var result = businessPartner.GetPhoneNumber(phoneNumber.Id);

        Assert.NotNull(result);
        Assert.Equal(
            PhoneNumberType.Residential,
            result.Type);
        Assert.Equal(
            "2133334444",
            result.Number);
    }

    [Fact]
    public void RemovePhoneNumber_ShouldRemovePhoneNumber_WhenItExists()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        businessPartner.AddPhoneNumber(
            PhoneNumberType.Mobile,
            "21999999999");

        var phoneNumber = businessPartner.PhoneNumbers.Single();

        // Act
        var removed = businessPartner.RemovePhoneNumber(phoneNumber.Id);

        // Assert
        Assert.True(removed);
        Assert.Empty(businessPartner.PhoneNumbers);
    }

    [Fact]
    public void UpdatePhoneNumber_ShouldReturnFalse_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        var result = businessPartner.UpdatePhoneNumber(
            Guid.NewGuid(),
            PhoneNumberType.Mobile,
            "21999999999");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemovePhoneNumber_ShouldReturnFalse_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        // Act
        var result = businessPartner.RemovePhoneNumber(
            Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}