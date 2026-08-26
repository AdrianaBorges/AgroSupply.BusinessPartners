using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class DeactivateBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public DeactivateBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartner?> ExecuteAsync(Guid id)
    {
        var businessPartner = await _repository.GetByIdAsync(id);

        if (businessPartner is null)
            return null;

        businessPartner.Deactivate();

        await _repository.UpdateAsync(businessPartner);

        return businessPartner;
    }
}