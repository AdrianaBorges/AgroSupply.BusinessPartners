using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class GetPhoneNumberFromBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public GetPhoneNumberFromBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneNumber?> ExecuteAsync(
        Guid businessPartnerId,
        Guid phoneNumberId)
    {
        var businessPartner =
            await _repository.GetByIdAsync(businessPartnerId);

        if (businessPartner is null)
            return null;

        return businessPartner.GetPhoneNumber(phoneNumberId);
    }
}