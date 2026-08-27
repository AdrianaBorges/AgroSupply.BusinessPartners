using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.Abstractions;

public interface IBusinessRelationshipRepository
{
    Task AddAsync(BusinessRelationship relationship);

    Task<BusinessRelationship?> GetByIdAsync(Guid id);

    Task UpdateAsync(BusinessRelationship relationship);

}