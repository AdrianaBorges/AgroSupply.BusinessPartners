using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;

public class CreateBusinessRelationshipUseCase
{
    private readonly IBusinessPartnerRepository _businessPartnerRepository;
    private readonly IBusinessRelationshipRepository _relationshipRepository;

    public CreateBusinessRelationshipUseCase(
        IBusinessPartnerRepository businessPartnerRepository,
        IBusinessRelationshipRepository relationshipRepository)
    {
        _businessPartnerRepository = businessPartnerRepository;
        _relationshipRepository = relationshipRepository;
    }

    public async Task<BusinessRelationship?> ExecuteAsync(
        Guid supplierBusinessPartnerId,
        Guid buyerBusinessPartnerId)
    {
        var supplier =
            await _businessPartnerRepository.GetByIdAsync(
                supplierBusinessPartnerId);

        if (supplier is null || !supplier.IsActive)
            return null;

        var buyer =
            await _businessPartnerRepository.GetByIdAsync(
                buyerBusinessPartnerId);

        if (buyer is null || !buyer.IsActive)
            return null;

        var relationshipExists =
            await _relationshipRepository.ExistsActiveAsync(
                supplierBusinessPartnerId,
                buyerBusinessPartnerId);

        if (relationshipExists)
            throw new InvalidOperationException(
                "Já existe um relacionamento comercial ativo entre os parceiros informados.");

        var relationship = new BusinessRelationship(
            supplierBusinessPartnerId,
            buyerBusinessPartnerId);

        await _relationshipRepository.AddAsync(relationship);

        return relationship;
    }
}