using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class UpdatePhoneNumberFromBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public UpdatePhoneNumberFromBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneNumber?> ExecuteAsync(
        Guid businessPartnerId,
        Guid phoneNumberId,
        PhoneNumberType type,
        string number)
    {
        var businessPartner =
            await _repository.GetByIdAsync(businessPartnerId);

        if (businessPartner is null)
            return null;

        var updated = businessPartner.UpdatePhoneNumber(
            phoneNumberId,
            type,
            number);

        if (!updated)
            return null;

        await _repository.UpdateAsync(businessPartner);

        return businessPartner.GetPhoneNumber(phoneNumberId);
    }
}