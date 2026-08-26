namespace AgroSupply.BusinessPartners.Api.Contracts.BusinessPartners;

public class UpdateBusinessPartnerRequest
{
    public string Name { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
}