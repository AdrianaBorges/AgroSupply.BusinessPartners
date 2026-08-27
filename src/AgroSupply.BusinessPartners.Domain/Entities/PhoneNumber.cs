using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Entities;

public class PhoneNumber
{
    private PhoneNumber()
    {
    }

    public PhoneNumber(
        PhoneNumberType type,
        string number)
    {
        ValidateNumber(number);

        Id = Guid.NewGuid();
        Type = type;
        Number = number;
    }

    public Guid Id { get; private set; }

    public PhoneNumberType Type { get; private set; }

    public string Number { get; private set; } = string.Empty;

    public void Update(
        PhoneNumberType type,
        string number)
    {
        ValidateNumber(number);

        Type = type;
        Number = number;
    }

    private static void ValidateNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException(
                "O número de telefone é obrigatório.",
                nameof(number));
    }
}