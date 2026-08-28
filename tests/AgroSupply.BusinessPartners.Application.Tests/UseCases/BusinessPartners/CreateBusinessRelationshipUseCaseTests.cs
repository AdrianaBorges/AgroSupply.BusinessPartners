using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessRelationships;

public class CreateBusinessRelationshipUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateRelationship_WhenPartnersExistAndAreActive()
    {
        // Arrange
        var supplier = new BusinessPartner(
            "Fornecedor Agro Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var buyer = new BusinessPartner(
            "Comprador Rural Ltda",
            "98765432100",
            new DateTime(1988, 3, 10));

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(
                supplier,
                buyer);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var result = await useCase.ExecuteAsync(
            supplier.Id,
            buyer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(
            supplier.Id,
            result.SupplierBusinessPartnerId);

        Assert.Equal(
            buyer.Id,
            result.BuyerBusinessPartnerId);

        Assert.Same(
            result,
            relationshipRepository.AddedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenSupplierDoesNotExist()
    {
        // Arrange
        var buyer = new BusinessPartner(
            "Comprador Rural Ltda",
            "98765432100",
            new DateTime(1988, 3, 10));

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(buyer);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var result = await useCase.ExecuteAsync(
            Guid.NewGuid(),
            buyer.Id);

        // Assert
        Assert.Null(result);
        Assert.Null(
            relationshipRepository.AddedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenBuyerDoesNotExist()
    {
        // Arrange
        var supplier = new BusinessPartner(
            "Fornecedor Agro Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(supplier);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var result = await useCase.ExecuteAsync(
            supplier.Id,
            Guid.NewGuid());

        // Assert
        Assert.Null(result);
        Assert.Null(
            relationshipRepository.AddedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenSupplierIsInactive()
    {
        // Arrange
        var supplier = new BusinessPartner(
            "Fornecedor Agro Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        supplier.Deactivate();

        var buyer = new BusinessPartner(
            "Comprador Rural Ltda",
            "98765432100",
            new DateTime(1988, 3, 10));

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(
                supplier,
                buyer);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var result = await useCase.ExecuteAsync(
            supplier.Id,
            buyer.Id);

        // Assert
        Assert.Null(result);
        Assert.Null(
            relationshipRepository.AddedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenBuyerIsInactive()
    {
        // Arrange
        var supplier = new BusinessPartner(
            "Fornecedor Agro Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var buyer = new BusinessPartner(
            "Comprador Rural Ltda",
            "98765432100",
            new DateTime(1988, 3, 10));

        buyer.Deactivate();

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(
                supplier,
                buyer);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var result = await useCase.ExecuteAsync(
            supplier.Id,
            buyer.Id);

        // Assert
        Assert.Null(result);
        Assert.Null(
            relationshipRepository.AddedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowInvalidOperationException_WhenActiveRelationshipAlreadyExists()
    {
        // Arrange
        var supplier = new BusinessPartner(
            "Fornecedor Agro Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var buyer = new BusinessPartner(
            "Comprador Rural Ltda",
            "98765432100",
            new DateTime(1988, 3, 10));

        var businessPartnerRepository =
            new FakeBusinessPartnerRepository(
                supplier,
                buyer);

        var relationshipRepository =
            new FakeBusinessRelationshipRepository
            {
                RelationshipExists = true
            };

        var useCase =
            new CreateBusinessRelationshipUseCase(
                businessPartnerRepository,
                relationshipRepository);

        // Act
        var exception =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => useCase.ExecuteAsync(
                    supplier.Id,
                    buyer.Id));

        // Assert
        Assert.Equal(
            "Já existe um relacionamento comercial ativo entre os parceiros informados.",
            exception.Message);

        Assert.Null(
            relationshipRepository.AddedRelationship);
    }

    private sealed class FakeBusinessPartnerRepository
        : IBusinessPartnerRepository
    {
        private readonly IReadOnlyCollection<BusinessPartner>
            _businessPartners;

        public FakeBusinessPartnerRepository(
            params BusinessPartner[] businessPartners)
        {
            _businessPartners = businessPartners;
        }

        public Task AddAsync(
            BusinessPartner businessPartner) =>
            Task.CompletedTask;

        public Task<IReadOnlyCollection<BusinessPartner>>
            GetAllAsync()
        {
            return Task.FromResult(
                _businessPartners);
        }

        public Task<BusinessPartner?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _businessPartners
                    .FirstOrDefault(x => x.Id == id));
        }

        public Task UpdateAsync(
            BusinessPartner businessPartner) =>
            Task.CompletedTask;
    }

    private sealed class FakeBusinessRelationshipRepository
      : IBusinessRelationshipRepository
    {
        public BusinessRelationship? AddedRelationship
        {
            get;
            private set;
        }

        public bool RelationshipExists
        {
            get;
            set;
        }

        public Task AddAsync(
            BusinessRelationship relationship)
        {
            AddedRelationship = relationship;

            return Task.CompletedTask;
        }

        public Task<BusinessRelationship?> GetByIdAsync(
            Guid id)
        {
            return Task.FromResult(
                AddedRelationship?.Id == id
                    ? AddedRelationship
                    : null);
        }

        public Task UpdateAsync(
            BusinessRelationship relationship)
        {
            AddedRelationship = relationship;

            return Task.CompletedTask;
        }

        public Task<bool> ExistsActiveAsync(
            Guid supplierBusinessPartnerId,
            Guid buyerBusinessPartnerId)
        {
            return Task.FromResult(
                RelationshipExists);
        }
    }
}