using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class GetBusinessPartnerByIdUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnBusinessPartner_WhenIdExists()
    {
        // Arrange
        const string name = "João da Silva";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        var businessPartner = new BusinessPartner(name, cpf, birthDate);
        var repository = new FakeBusinessPartnerRepository(businessPartner);
        var useCase = new GetBusinessPartnerByIdUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(businessPartner.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(businessPartner.Id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(cpf, result.Cpf);
    }

    private sealed class FakeBusinessPartnerRepository : IBusinessPartnerRepository
    {
        private readonly BusinessPartner? _businessPartner;

        public FakeBusinessPartnerRepository(BusinessPartner? businessPartner)
        {
            _businessPartner = businessPartner;
        }

        public Task AddAsync(BusinessPartner businessPartner)
        {
            return Task.CompletedTask;
        }

        public Task<BusinessPartner?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _businessPartner?.Id == id
                    ? _businessPartner
                    : null);
        }
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var repository = new FakeBusinessPartnerRepository(null);
        var useCase = new GetBusinessPartnerByIdUseCase(repository);
        var id = Guid.NewGuid();

        // Act
        var result = await useCase.ExecuteAsync(id);

        // Assert
        Assert.Null(result);
    }
}