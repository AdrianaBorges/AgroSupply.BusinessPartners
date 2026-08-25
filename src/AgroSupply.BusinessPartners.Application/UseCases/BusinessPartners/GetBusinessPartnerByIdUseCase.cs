using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;

public class GetBusinessPartnerByIdUseCase
{
    private readonly IBusinessPartnerRepository _repository;

    public GetBusinessPartnerByIdUseCase(IBusinessPartnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<BusinessPartner?> ExecuteAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}