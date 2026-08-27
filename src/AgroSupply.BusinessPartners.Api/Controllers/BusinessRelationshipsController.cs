using AgroSupply.BusinessPartners.Api.Contracts.BusinessRelationships;
using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;
using Microsoft.AspNetCore.Mvc;

namespace AgroSupply.BusinessPartners.Api.Controllers;

[ApiController]
[Route("api/business-relationships")]
public class BusinessRelationshipsController : ControllerBase
{
    private readonly CreateBusinessRelationshipUseCase _createBusinessRelationshipUseCase;
    private readonly DeactivateBusinessRelationshipUseCase _deactivateBusinessRelationshipUseCase;
    private readonly IBusinessRelationshipRepository _relationshipRepository;

    public BusinessRelationshipsController(
       CreateBusinessRelationshipUseCase createBusinessRelationshipUseCase,
       DeactivateBusinessRelationshipUseCase deactivateBusinessRelationshipUseCase,
       IBusinessRelationshipRepository relationshipRepository)
    {
        _createBusinessRelationshipUseCase = createBusinessRelationshipUseCase;
        _deactivateBusinessRelationshipUseCase = deactivateBusinessRelationshipUseCase;
        _relationshipRepository = relationshipRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBusinessRelationshipRequest request)
    {
        var relationship = await _createBusinessRelationshipUseCase.ExecuteAsync(
            request.SupplierBusinessPartnerId,
            request.BuyerBusinessPartnerId);

        if (relationship is null)
            return NotFound();

        return CreatedAtAction(
            nameof(GetById),
            new { id = relationship.Id },
            relationship);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var relationship = await _relationshipRepository.GetByIdAsync(id);

        if (relationship is null)
            return NotFound();

        return Ok(relationship);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result =
            await _deactivateBusinessRelationshipUseCase.ExecuteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}