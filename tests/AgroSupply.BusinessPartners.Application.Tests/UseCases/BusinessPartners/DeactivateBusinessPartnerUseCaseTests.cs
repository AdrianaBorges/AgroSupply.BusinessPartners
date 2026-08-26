using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class DeactivateBusinessPartnerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldDeactivateBusinessPartner_WhenIdExists()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var repository = new FakeBusinessPartnerRepository(businessPartner);
        var useCase = new DeactivateBusinessPartnerUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(businessPartner.Id);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsActive);
        Assert.NotNull(result.DeactivatedAt);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var repository = new FakeBusinessPartnerRepository(null);
        var useCase = new DeactivateBusinessPartnerUseCase(repository);

        var id = Guid.NewGuid();

        // Act
        var result = await useCase.ExecuteAsync(id);

        // Assert
        Assert.Null(result);
    }

    private sealed class FakeBusinessPartnerRepository
        : IBusinessPartnerRepository
    {
        private BusinessPartner? _businessPartner;

        public FakeBusinessPartnerRepository(
            BusinessPartner? businessPartner)
        {
            _businessPartner = businessPartner;
        }

        public Task AddAsync(BusinessPartner businessPartner)
        {
            _businessPartner = businessPartner;

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<BusinessPartner>> GetAllAsync()
        {
            IReadOnlyCollection<BusinessPartner> result =
                _businessPartner is null
                    ? Array.Empty<BusinessPartner>()
                    : new[] { _businessPartner };

            return Task.FromResult(result);
        }

        public Task<BusinessPartner?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _businessPartner?.Id == id
                    ? _businessPartner
                    : null);
        }

        public Task UpdateAsync(BusinessPartner businessPartner)
        {
            _businessPartner = businessPartner;

            return Task.CompletedTask;
        }
    }
}