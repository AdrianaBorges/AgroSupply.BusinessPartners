using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class AddPhoneNumberToBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public AddPhoneNumberToBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartner?> ExecuteAsync(
        Guid businessPartnerId,
        PhoneNumberType type,
        string number)
    {
        var businessPartner =
            await _repository.GetByIdAsync(businessPartnerId);

        if (businessPartner is null)
            return null;

        businessPartner.AddPhoneNumber(type, number);

        await _repository.UpdateAsync(businessPartner);

        return businessPartner;
    }
}