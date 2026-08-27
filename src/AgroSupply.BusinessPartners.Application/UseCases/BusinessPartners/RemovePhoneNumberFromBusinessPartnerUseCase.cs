using AgroSupply.BusinessPartners.Application.Abstractions;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class RemovePhoneNumberFromBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public RemovePhoneNumberFromBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        Guid businessPartnerId,
        Guid phoneNumberId)
    {
        var businessPartner =
            await _repository.GetByIdAsync(businessPartnerId);

        if (businessPartner is null)
            return false;

        var removed =
            businessPartner.RemovePhoneNumber(phoneNumberId);

        if (!removed)
            return false;

        await _repository.UpdateAsync(businessPartner);

        return true;
    }
}