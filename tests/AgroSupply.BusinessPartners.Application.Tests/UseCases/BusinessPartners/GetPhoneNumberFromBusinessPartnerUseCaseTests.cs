using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class GetPhoneNumberFromBusinessPartnerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnPhoneNumber_WhenItExists()
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

        var repository =
            new FakeBusinessPartnerRepository(businessPartner);

        var useCase =
            new GetPhoneNumberFromBusinessPartnerUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(
            businessPartner.Id,
            phoneNumber.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phoneNumber.Id, result.Id);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenPhoneNumberDoesNotExist()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Teste Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var repository =
            new FakeBusinessPartnerRepository(businessPartner);

        var useCase =
            new GetPhoneNumberFromBusinessPartnerUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(
            businessPartner.Id,
            Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    private sealed class FakeBusinessPartnerRepository
        : IBusinessPartnerRepository
    {
        private readonly BusinessPartner? _businessPartner;

        public FakeBusinessPartnerRepository(
            BusinessPartner? businessPartner)
        {
            _businessPartner = businessPartner;
        }

        public Task AddAsync(BusinessPartner businessPartner) =>
            Task.CompletedTask;

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

        public Task UpdateAsync(BusinessPartner businessPartner) =>
            Task.CompletedTask;
    }
}