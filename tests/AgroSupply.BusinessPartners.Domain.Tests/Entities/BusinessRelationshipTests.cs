using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Tests.Entities;

public class BusinessRelationshipTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveRelationship_WhenDataIsValid()
    {
        // Arrange
        var supplierBusinessPartnerId = Guid.NewGuid();
        var buyerBusinessPartnerId = Guid.NewGuid();

        // Act
        var relationship = new BusinessRelationship(
            supplierBusinessPartnerId,
            buyerBusinessPartnerId);

        // Assert
        Assert.NotEqual(Guid.Empty, relationship.Id);
        Assert.Equal(
            supplierBusinessPartnerId,
            relationship.SupplierBusinessPartnerId);
        Assert.Equal(
            buyerBusinessPartnerId,
            relationship.BuyerBusinessPartnerId);
        Assert.Equal(
            BusinessRelationshipStatus.Active,
            relationship.Status);
        Assert.NotEqual(default, relationship.CreatedAt);
        Assert.Null(relationship.DeactivatedAt);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSupplierIsEmpty()
    {
        // Arrange
        var buyerBusinessPartnerId = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new BusinessRelationship(
                Guid.Empty,
                buyerBusinessPartnerId));

        // Assert
        Assert.Equal(
            "supplierBusinessPartnerId",
            exception.ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenBuyerIsEmpty()
    {
        // Arrange
        var supplierBusinessPartnerId = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new BusinessRelationship(
                supplierBusinessPartnerId,
                Guid.Empty));

        // Assert
        Assert.Equal(
            "buyerBusinessPartnerId",
            exception.ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenSupplierAndBuyerAreTheSame()
    {
        // Arrange
        var businessPartnerId = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new BusinessRelationship(
                businessPartnerId,
                businessPartnerId));

        // Assert
        Assert.Equal(
            "Um parceiro de negócio não pode estabelecer uma relação comercial consigo mesmo.",
            exception.Message);
    }

    [Fact]
    public void Deactivate_ShouldSetStatusToInactiveAndSetDeactivatedAt()
    {
        // Arrange
        var relationship = new BusinessRelationship(
            Guid.NewGuid(),
            Guid.NewGuid());

        // Act
        relationship.Deactivate();

        // Assert
        Assert.Equal(
            BusinessRelationshipStatus.Inactive,
            relationship.Status);
        Assert.NotNull(relationship.DeactivatedAt);
    }

    [Fact]
    public void Deactivate_ShouldKeepDeactivationData_WhenAlreadyInactive()
    {
        // Arrange
        var relationship = new BusinessRelationship(
            Guid.NewGuid(),
            Guid.NewGuid());

        relationship.Deactivate();

        var deactivatedAt = relationship.DeactivatedAt;

        // Act
        relationship.Deactivate();

        // Assert
        Assert.Equal(
            BusinessRelationshipStatus.Inactive,
            relationship.Status);
        Assert.Equal(
            deactivatedAt,
            relationship.DeactivatedAt);
    }
}