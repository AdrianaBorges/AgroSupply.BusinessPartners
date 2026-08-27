using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessPartners;

public class AddPhoneNumberToBusinessPartnerUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldAddPhoneNumber_WhenBusinessPartnerExists()
    {
        // Arrange
        var businessPartner = new BusinessPartner(
            "Agro Forte Ltda",
            "12345678901",
            new DateTime(1990, 5, 15));

        var repository = new FakeBusinessPartnerRepository(businessPartner);
        var useCase = new AddPhoneNumberToBusinessPartnerUseCase(repository);

        const PhoneNumberType type = PhoneNumberType.Mobile;
        const string number = "21999999999";

        // Act
        var result = await useCase.ExecuteAsync(
            businessPartner.Id,
            type,
            number);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.PhoneNumbers);

        var phoneNumber = result.PhoneNumbers.Single();

        Assert.Equal(type, phoneNumber.Type);
        Assert.Equal(number, phoneNumber.Number);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnNull_WhenBusinessPartnerDoesNotExist()
    {
        // Arrange
        var repository = new FakeBusinessPartnerRepository(null);
        var useCase = new AddPhoneNumberToBusinessPartnerUseCase(repository);

        var id = Guid.NewGuid();

        // Act
        var result = await useCase.ExecuteAsync(
            id,
            PhoneNumberType.Mobile,
            "21999999999");

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