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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
         CreateBusinessRelationshipRequest request)
    {
        try
        {
            var relationship =
                await _createBusinessRelationshipUseCase.ExecuteAsync(
                    request.SupplierBusinessPartnerId,
                    request.BuyerBusinessPartnerId);

            if (relationship is null)
                return NotFound();

            return CreatedAtAction(
                nameof(GetById),
                new { id = relationship.Id },
                relationship);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var relationship = await _relationshipRepository.GetByIdAsync(id);

        if (relationship is null)
            return NotFound();

        return Ok(relationship);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result =
            await _deactivateBusinessRelationshipUseCase.ExecuteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}