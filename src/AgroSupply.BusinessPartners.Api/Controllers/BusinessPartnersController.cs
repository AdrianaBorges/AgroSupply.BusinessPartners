using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using AgroSupply.BusinessPartners.Api.Contracts.BusinessPartners;
using Microsoft.AspNetCore.Mvc;

namespace AgroSupply.BusinessPartners.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessPartnersController : ControllerBase
{
    private readonly CreateBusinessPartnerUseCase _createBusinessPartnerUseCase;
    private readonly GetBusinessPartnerByIdUseCase _getBusinessPartnerByIdUseCase;
    private readonly GetAllBusinessPartnersUseCase _getAllBusinessPartnersUseCase;
    private readonly UpdateBusinessPartnerUseCase _updateBusinessPartnerUseCase;
    private readonly DeactivateBusinessPartnerUseCase _deactivateBusinessPartnerUseCase;

    public BusinessPartnersController(
        CreateBusinessPartnerUseCase createBusinessPartnerUseCase,
        GetBusinessPartnerByIdUseCase getBusinessPartnerByIdUseCase,
        GetAllBusinessPartnersUseCase getAllBusinessPartnersUseCase,
        UpdateBusinessPartnerUseCase updateBusinessPartnerUseCase,
        DeactivateBusinessPartnerUseCase deactivateBusinessPartnerUseCase)
    {
        _createBusinessPartnerUseCase = createBusinessPartnerUseCase;
        _getBusinessPartnerByIdUseCase = getBusinessPartnerByIdUseCase;
        _getAllBusinessPartnersUseCase = getAllBusinessPartnersUseCase;
        _updateBusinessPartnerUseCase = updateBusinessPartnerUseCase;
        _deactivateBusinessPartnerUseCase = deactivateBusinessPartnerUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateBusinessPartnerRequest request)
    {
        var businessPartner = await _createBusinessPartnerUseCase.ExecuteAsync(
            request.Name,
            request.Cpf,
            request.BirthDate);

        return CreatedAtAction(
            nameof(Create),
            new { id = businessPartner.Id },
            businessPartner);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var businessPartner =
            await _getBusinessPartnerByIdUseCase.ExecuteAsync(id);

        if (businessPartner is null)
            return NotFound();

        return Ok(businessPartner);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var businessPartners =
            await _getAllBusinessPartnersUseCase.ExecuteAsync();

        return Ok(businessPartners);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateBusinessPartnerRequest request)
    {
        var businessPartner =
            await _updateBusinessPartnerUseCase.ExecuteAsync(
                id,
                request.Name,
                request.Cpf,
                request.BirthDate);

        if (businessPartner is null)
            return NotFound();

        return Ok(businessPartner);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var businessPartner =
            await _deactivateBusinessPartnerUseCase.ExecuteAsync(id);

        if (businessPartner is null)
            return NotFound();

        return NoContent();
    }
}