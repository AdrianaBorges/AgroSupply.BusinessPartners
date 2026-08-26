using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class GetAllBusinessPartnersUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public GetAllBusinessPartnersUseCase(
        IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<BusinessPartner>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}