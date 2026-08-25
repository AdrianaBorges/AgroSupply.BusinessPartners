using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class CreateBusinessPartnerUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public CreateBusinessPartnerUseCase(IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartner> ExecuteAsync(
        string name,
        string cpf,
        DateTime birthDate)
    {
        var businessPartner = new BusinessPartner(name, cpf, birthDate);

        await _repository.AddAsync(businessPartner);

        return businessPartner;
    }
}