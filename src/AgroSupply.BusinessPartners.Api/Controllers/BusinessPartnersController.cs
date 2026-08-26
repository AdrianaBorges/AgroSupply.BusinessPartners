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

    public BusinessPartnersController(
    CreateBusinessPartnerUseCase createBusinessPartnerUseCase,
    GetBusinessPartnerByIdUseCase getBusinessPartnerByIdUseCase)
    {
        _createBusinessPartnerUseCase = createBusinessPartnerUseCase;
        _getBusinessPartnerByIdUseCase = getBusinessPartnerByIdUseCase;
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

}


