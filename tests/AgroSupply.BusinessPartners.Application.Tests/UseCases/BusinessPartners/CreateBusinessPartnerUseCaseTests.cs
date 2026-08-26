using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class CreateBusinessPartnerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCreateBusinessPartner_WhenDataIsValid()
    {
        // Arrange
        var repository = new FakeBusinessPartnerRepository();
        var useCase = new CreateBusinessPartnerUseCase(repository);

        const string name = "João da Silva";
        const string cpf = "12345678901";
        var birthDate = new DateTime(1990, 5, 15);

        // Act
        var result = await useCase.ExecuteAsync(name, cpf, birthDate);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(cpf, result.Cpf);
        Assert.Equal(birthDate, result.BirthDate);
        Assert.True(result.IsActive);
    }

    private sealed class FakeBusinessPartnerRepository : IBusinessPartnerRepository
    {
        public BusinessPartner? BusinessPartner { get; private set; }

        public Task AddAsync(BusinessPartner businessPartner)
        {
            BusinessPartner = businessPartner;

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<BusinessPartner>> GetAllAsync()
        {
            IReadOnlyCollection<BusinessPartner> businessPartners =
                BusinessPartner is null
                    ? Array.Empty<BusinessPartner>()
                    : new[] { BusinessPartner };

            return Task.FromResult(businessPartners);
        }

        public Task<BusinessPartner?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                BusinessPartner?.Id == id
                    ? BusinessPartner
                    : null);
        }
    }
}