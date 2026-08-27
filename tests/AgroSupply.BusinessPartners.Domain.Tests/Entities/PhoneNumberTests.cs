using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Tests.Entities;

public class PhoneNumberTests
{
    [Fact]
    public void Constructor_ShouldCreatePhoneNumber_WhenDataIsValid()
    {
        // Arrange
        const string number = "21999999999";
        const PhoneNumberType type = PhoneNumberType.Mobile;

        // Act
        var phoneNumber = new PhoneNumber(type, number);

        // Assert
        Assert.NotEqual(Guid.Empty, phoneNumber.Id);
        Assert.Equal(type, phoneNumber.Type);
        Assert.Equal(number, phoneNumber.Number);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNumberIsEmpty()
    {
        // Arrange
        const string number = "";
        const PhoneNumberType type = PhoneNumberType.Mobile;

        // Act
        Action action = () => new PhoneNumber(type, number);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenNumberIsWhiteSpace()
    {
        // Arrange
        const string number = "   ";
        const PhoneNumberType type = PhoneNumberType.Mobile;

        // Act
        Action action = () => new PhoneNumber(type, number);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}