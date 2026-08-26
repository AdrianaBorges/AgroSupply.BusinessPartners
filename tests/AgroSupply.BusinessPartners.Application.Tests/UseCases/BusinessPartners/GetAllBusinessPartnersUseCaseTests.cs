using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class GetAllBusinessPartnersUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnAllBusinessPartners()
    {
        // Arrange
        var repository = new FakeBusinessPartnerRepository();

        var firstBusinessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var secondBusinessPartner = new BusinessPartner(
            "Campo Verde Distribuidora Ltda",
            "98765432100",
            new DateTime(1985, 10, 20));

        await repository.AddAsync(firstBusinessPartner);
        await repository.AddAsync(secondBusinessPartner);

        var useCase = new GetAllBusinessPartnersUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(firstBusinessPartner, result);
        Assert.Contains(secondBusinessPartner, result);
    }

    private sealed class FakeBusinessPartnerRepository : IBusinessPartnerRepository
    {
        private readonly List<BusinessPartner> _businessPartners = new();

        public Task AddAsync(BusinessPartner businessPartner)
        {
            _businessPartners.Add(businessPartner);

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<BusinessPartner>> GetAllAsync()
        {
            IReadOnlyCollection<BusinessPartner> result =
                _businessPartners.ToList();

            return Task.FromResult(result);
        }

        public Task<BusinessPartner?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _businessPartners.FirstOrDefault(x => x.Id == id));
        }

        public Task UpdateAsync(BusinessPartner businessPartner)
        {
            var index = _businessPartners.FindIndex(
                x => x.Id == businessPartner.Id);

            if (index >= 0)
                _businessPartners[index] = businessPartner;

            return Task.CompletedTask;
        }
    }
}