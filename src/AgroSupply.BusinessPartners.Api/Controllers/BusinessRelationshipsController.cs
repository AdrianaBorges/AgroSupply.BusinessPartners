using Microsoft.AspNetCore.Authorization;
using AgroSupply.BusinessPartners.Api.Contracts.BusinessRelationships;
using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;
using Microsoft.AspNetCore.Mvc;

namespace AgroSupply.BusinessPartners.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/business-relationships")]
public class BusinessRelationshipsController : ControllerBase
{
    private readonly CreateBusinessRelationshipUseCase _createBusinessRelationshipUseCase;
    private readonly DeactivateBusinessRelationshipUseCase _deactivateBusinessRelationshipUseCase;
    private readonly IBusinessRelationshipRepository _relationshipRepository;
    private readonly ILogger<BusinessRelationshipsController> _logger;

    public BusinessRelationshipsController(
        CreateBusinessRelationshipUseCase createBusinessRelationshipUseCase,
        DeactivateBusinessRelationshipUseCase deactivateBusinessRelationshipUseCase,
        IBusinessRelationshipRepository relationshipRepository,
        ILogger<BusinessRelationshipsController> logger)
    {
        _createBusinessRelationshipUseCase = createBusinessRelationshipUseCase;
        _deactivateBusinessRelationshipUseCase = deactivateBusinessRelationshipUseCase;
        _relationshipRepository = relationshipRepository;
        _logger = logger;
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
            _logger.LogInformation(
                "Iniciando criação de relacionamento comercial entre Supplier {SupplierBusinessPartnerId} e Buyer {BuyerBusinessPartnerId}.",
                request.SupplierBusinessPartnerId,
                request.BuyerBusinessPartnerId);

            var relationship =
                await _createBusinessRelationshipUseCase.ExecuteAsync(
                    request.SupplierBusinessPartnerId,
                    request.BuyerBusinessPartnerId);

            if (relationship is null)
            {
                _logger.LogWarning(
                    "Não foi possível criar o relacionamento comercial. Supplier {SupplierBusinessPartnerId} ou Buyer {BuyerBusinessPartnerId} inexistente ou inativo.",
                    request.SupplierBusinessPartnerId,
                    request.BuyerBusinessPartnerId);

                return NotFound();
            }

            _logger.LogInformation(
                "Relacionamento comercial {BusinessRelationshipId} criado com sucesso entre Supplier {SupplierBusinessPartnerId} e Buyer {BuyerBusinessPartnerId}.",
                relationship.Id,
                request.SupplierBusinessPartnerId,
                request.BuyerBusinessPartnerId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = relationship.Id },
                relationship);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Falha de validação ao criar relacionamento comercial entre Supplier {SupplierBusinessPartnerId} e Buyer {BuyerBusinessPartnerId}.",
                request.SupplierBusinessPartnerId,
                request.BuyerBusinessPartnerId);

            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(
                ex,
                "Conflito ao criar relacionamento comercial entre Supplier {SupplierBusinessPartnerId} e Buyer {BuyerBusinessPartnerId}.",
                request.SupplierBusinessPartnerId,
                request.BuyerBusinessPartnerId);

            return Conflict(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var relationship =
            await _relationshipRepository.GetByIdAsync(id);

        if (relationship is null)
        {
            _logger.LogWarning(
                "Relacionamento comercial {BusinessRelationshipId} não encontrado.",
                id);

            return NotFound();
        }

        return Ok(relationship);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation(
            "Iniciando desativação do relacionamento comercial {BusinessRelationshipId}.",
            id);

        var result =
            await _deactivateBusinessRelationshipUseCase.ExecuteAsync(id);

        if (!result)
        {
            _logger.LogWarning(
                "Relacionamento comercial {BusinessRelationshipId} não encontrado para desativação.",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Relacionamento comercial {BusinessRelationshipId} desativado com sucesso.",
            id);

        return NoContent();
    }
}