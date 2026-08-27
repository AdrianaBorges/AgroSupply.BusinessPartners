using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Entities;

public class PhoneNumber
{
    public PhoneNumber(
        PhoneNumberType type,
        string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException(
                "O número de telefone é obrigatório.",
                nameof(number));

        Id = Guid.NewGuid();
        Type = type;
        Number = number;
    }

    public Guid Id { get; }

    public PhoneNumberType Type { get; }

    public string Number { get; }
}