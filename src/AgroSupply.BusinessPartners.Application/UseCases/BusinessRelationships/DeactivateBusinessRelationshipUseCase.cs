using AgroSupply.BusinessPartners.Application.Abstractions;

namespace AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;

public class DeactivateBusinessRelationshipUseCase
{
    private readonly IBusinessRelationshipRepository _relationshipRepository;

    public DeactivateBusinessRelationshipUseCase(
        IBusinessRelationshipRepository relationshipRepository)
    {
        _relationshipRepository = relationshipRepository;
    }

    public async Task<bool> ExecuteAsync(Guid id)
    {
        var relationship =
            await _relationshipRepository.GetByIdAsync(id);

        if (relationship is null)
            return false;

        relationship.Deactivate();

        await _relationshipRepository.UpdateAsync(relationship);

        return true;
    }
}