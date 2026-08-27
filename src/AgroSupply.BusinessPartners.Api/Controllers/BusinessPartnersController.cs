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
    private readonly AddPhoneNumberToBusinessPartnerUseCase _addPhoneNumberToBusinessPartnerUseCase;

    public BusinessPartnersController(
        CreateBusinessPartnerUseCase createBusinessPartnerUseCase,
        GetBusinessPartnerByIdUseCase getBusinessPartnerByIdUseCase,
        GetAllBusinessPartnersUseCase getAllBusinessPartnersUseCase,
        UpdateBusinessPartnerUseCase updateBusinessPartnerUseCase,
        DeactivateBusinessPartnerUseCase deactivateBusinessPartnerUseCase,
        AddPhoneNumberToBusinessPartnerUseCase addPhoneNumberToBusinessPartnerUseCase)
    {
        _createBusinessPartnerUseCase = createBusinessPartnerUseCase;
        _getBusinessPartnerByIdUseCase = getBusinessPartnerByIdUseCase;
        _getAllBusinessPartnersUseCase = getAllBusinessPartnersUseCase;
        _updateBusinessPartnerUseCase = updateBusinessPartnerUseCase;
        _deactivateBusinessPartnerUseCase = deactivateBusinessPartnerUseCase;
        _addPhoneNumberToBusinessPartnerUseCase = addPhoneNumberToBusinessPartnerUseCase;
    }

    /// <summary>
    /// Creates a new Business Partner.
    /// </summary>
    /// <param name="request">Business Partner data.</param>
    /// <returns>The created Business Partner.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Gets a Business Partner by identifier.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <returns>The Business Partner when found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var businessPartner =
            await _getBusinessPartnerByIdUseCase.ExecuteAsync(id);

        if (businessPartner is null)
            return NotFound();

        return Ok(businessPartner);
    }

    /// <summary>
    /// Gets all Business Partners.
    /// </summary>
    /// <returns>The list of Business Partners.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var businessPartners =
            await _getAllBusinessPartnersUseCase.ExecuteAsync();

        return Ok(businessPartners);
    }

    /// <summary>
    /// Updates an existing Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <param name="request">Updated Business Partner data.</param>
    /// <returns>The updated Business Partner.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Deactivates a Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <returns>No content when the Business Partner is successfully deactivated.</returns>
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

    /// <summary>
    /// Adds a phone number to an existing Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <param name="request">Phone number data.</param>
    /// <returns>The Business Partner with the new phone number.</returns>
    [HttpPost("{id:guid}/phone-numbers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPhoneNumber(
        Guid id,
        AddPhoneNumberRequest request)
    {
        var businessPartner =
            await _addPhoneNumberToBusinessPartnerUseCase.ExecuteAsync(
                id,
                request.Type,
                request.Number);

        if (businessPartner is null)
            return NotFound();

        return Ok(businessPartner);
    }
}