using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Api.Contracts.BusinessPartners;

public class AddPhoneNumberRequest
{
    public PhoneNumberType Type { get; set; }

    public string Number { get; set; } = string.Empty;
}