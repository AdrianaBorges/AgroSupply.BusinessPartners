using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Abstractions;

public interface IBusinessPartnerRepository
{
    Task AddAsync(BusinessPartner businessPartner);
    Task<BusinessPartner?> GetByIdAsync(Guid id);
}