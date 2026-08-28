using Microsoft.AspNetCore.Authorization;
using AgroSupply.BusinessPartners.Api.Contracts.BusinessPartners;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessPartners;
using Microsoft.AspNetCore.Mvc;

namespace AgroSupply.BusinessPartners.Api.Controllers;

[Authorize]
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
    private readonly GetPhoneNumberFromBusinessPartnerUseCase _getPhoneNumberFromBusinessPartnerUseCase;
    private readonly UpdatePhoneNumberFromBusinessPartnerUseCase _updatePhoneNumberFromBusinessPartnerUseCase;
    private readonly RemovePhoneNumberFromBusinessPartnerUseCase _removePhoneNumberFromBusinessPartnerUseCase;
    private readonly ILogger<BusinessPartnersController> _logger;

    public BusinessPartnersController(
        CreateBusinessPartnerUseCase createBusinessPartnerUseCase,
        GetBusinessPartnerByIdUseCase getBusinessPartnerByIdUseCase,
        GetAllBusinessPartnersUseCase getAllBusinessPartnersUseCase,
        UpdateBusinessPartnerUseCase updateBusinessPartnerUseCase,
        DeactivateBusinessPartnerUseCase deactivateBusinessPartnerUseCase,
        AddPhoneNumberToBusinessPartnerUseCase addPhoneNumberToBusinessPartnerUseCase,
        GetPhoneNumberFromBusinessPartnerUseCase getPhoneNumberFromBusinessPartnerUseCase,
        UpdatePhoneNumberFromBusinessPartnerUseCase updatePhoneNumberFromBusinessPartnerUseCase,
        RemovePhoneNumberFromBusinessPartnerUseCase removePhoneNumberFromBusinessPartnerUseCase,
        ILogger<BusinessPartnersController> logger)
    {
        _createBusinessPartnerUseCase = createBusinessPartnerUseCase;
        _getBusinessPartnerByIdUseCase = getBusinessPartnerByIdUseCase;
        _getAllBusinessPartnersUseCase = getAllBusinessPartnersUseCase;
        _updateBusinessPartnerUseCase = updateBusinessPartnerUseCase;
        _deactivateBusinessPartnerUseCase = deactivateBusinessPartnerUseCase;
        _addPhoneNumberToBusinessPartnerUseCase = addPhoneNumberToBusinessPartnerUseCase;
        _getPhoneNumberFromBusinessPartnerUseCase = getPhoneNumberFromBusinessPartnerUseCase;
        _updatePhoneNumberFromBusinessPartnerUseCase = updatePhoneNumberFromBusinessPartnerUseCase;
        _removePhoneNumberFromBusinessPartnerUseCase = removePhoneNumberFromBusinessPartnerUseCase;
        _logger = logger;
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
        _logger.LogInformation(
            "Iniciando criação de Business Partner.");

        var businessPartner =
            await _createBusinessPartnerUseCase.ExecuteAsync(
                request.Name,
                request.Cpf,
                request.BirthDate);

        _logger.LogInformation(
            "Business Partner {BusinessPartnerId} criado com sucesso.",
            businessPartner.Id);

        return CreatedAtAction(
            nameof(GetById),
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
        {
            _logger.LogWarning(
                "Business Partner {BusinessPartnerId} não encontrado.",
                id);

            return NotFound();
        }

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
        _logger.LogInformation(
            "Iniciando atualização do Business Partner {BusinessPartnerId}.",
            id);

        var businessPartner =
            await _updateBusinessPartnerUseCase.ExecuteAsync(
                id,
                request.Name,
                request.Cpf,
                request.BirthDate);

        if (businessPartner is null)
        {
            _logger.LogWarning(
                "Business Partner {BusinessPartnerId} não encontrado para atualização.",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Business Partner {BusinessPartnerId} atualizado com sucesso.",
            id);

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
        _logger.LogInformation(
            "Iniciando inativação do Business Partner {BusinessPartnerId}.",
            id);

        var businessPartner =
            await _deactivateBusinessPartnerUseCase.ExecuteAsync(id);

        if (businessPartner is null)
        {
            _logger.LogWarning(
                "Business Partner {BusinessPartnerId} não encontrado para inativação.",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Business Partner {BusinessPartnerId} inativado com sucesso.",
            id);

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
        _logger.LogInformation(
            "Iniciando inclusão de telefone no Business Partner {BusinessPartnerId}.",
            id);

        var businessPartner =
            await _addPhoneNumberToBusinessPartnerUseCase.ExecuteAsync(
                id,
                request.Type,
                request.Number);

        if (businessPartner is null)
        {
            _logger.LogWarning(
                "Business Partner {BusinessPartnerId} não encontrado para inclusão de telefone.",
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Telefone incluído com sucesso no Business Partner {BusinessPartnerId}.",
            id);

        return Ok(businessPartner);
    }

    /// <summary>
    /// Gets a phone number from an existing Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <param name="phoneNumberId">Phone number identifier.</param>
    /// <returns>The phone number when found.</returns>
    [HttpGet("{id:guid}/phone-numbers/{phoneNumberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPhoneNumber(
        Guid id,
        Guid phoneNumberId)
    {
        var phoneNumber =
            await _getPhoneNumberFromBusinessPartnerUseCase.ExecuteAsync(
                id,
                phoneNumberId);

        if (phoneNumber is null)
        {
            _logger.LogWarning(
                "Telefone {PhoneNumberId} não encontrado no Business Partner {BusinessPartnerId}.",
                phoneNumberId,
                id);

            return NotFound();
        }

        return Ok(phoneNumber);
    }

    /// <summary>
    /// Updates a phone number from an existing Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <param name="phoneNumberId">Phone number identifier.</param>
    /// <param name="request">Updated phone number data.</param>
    /// <returns>The updated phone number.</returns>
    [HttpPut("{id:guid}/phone-numbers/{phoneNumberId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePhoneNumber(
        Guid id,
        Guid phoneNumberId,
        UpdatePhoneNumberRequest request)
    {
        _logger.LogInformation(
            "Iniciando atualização do telefone {PhoneNumberId} do Business Partner {BusinessPartnerId}.",
            phoneNumberId,
            id);

        var phoneNumber =
            await _updatePhoneNumberFromBusinessPartnerUseCase.ExecuteAsync(
                id,
                phoneNumberId,
                request.Type,
                request.Number);

        if (phoneNumber is null)
        {
            _logger.LogWarning(
                "Telefone {PhoneNumberId} não encontrado no Business Partner {BusinessPartnerId} para atualização.",
                phoneNumberId,
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Telefone {PhoneNumberId} do Business Partner {BusinessPartnerId} atualizado com sucesso.",
            phoneNumberId,
            id);

        return Ok(phoneNumber);
    }

    /// <summary>
    /// Removes a phone number from an existing Business Partner.
    /// </summary>
    /// <param name="id">Business Partner identifier.</param>
    /// <param name="phoneNumberId">Phone number identifier.</param>
    /// <returns>No content when the phone number is successfully removed.</returns>
    [HttpDelete("{id:guid}/phone-numbers/{phoneNumberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePhoneNumber(
        Guid id,
        Guid phoneNumberId)
    {
        _logger.LogInformation(
            "Iniciando remoção do telefone {PhoneNumberId} do Business Partner {BusinessPartnerId}.",
            phoneNumberId,
            id);

        var removed =
            await _removePhoneNumberFromBusinessPartnerUseCase.ExecuteAsync(
                id,
                phoneNumberId);

        if (!removed)
        {
            _logger.LogWarning(
                "Telefone {PhoneNumberId} não encontrado no Business Partner {BusinessPartnerId} para remoção.",
                phoneNumberId,
                id);

            return NotFound();
        }

        _logger.LogInformation(
            "Telefone {PhoneNumberId} removido com sucesso do Business Partner {BusinessPartnerId}.",
            phoneNumberId,
            id);

        return NoContent();
    }
}