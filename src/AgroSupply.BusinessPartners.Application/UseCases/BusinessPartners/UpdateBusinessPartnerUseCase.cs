using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class UpdateBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public UpdateBusinessPartnerUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartner?> ExecuteAsync(
        Guid id,
        string name,
        string cpf,
        DateTime birthDate)
    {
        var businessPartner = await _repository.GetByIdAsync(id);

        if (businessPartner is null)
            return null;

        businessPartner.Update(name, cpf, birthDate);

        await _repository.UpdateAsync(businessPartner);

        return businessPartner;
    }
}