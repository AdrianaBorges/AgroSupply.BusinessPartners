using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Entities;

public class BusinessPartner
{
    private readonly List<PhoneNumber> _phoneNumbers = new();

    public BusinessPartner(string name, string cpf, DateTime birthDate)
    {
        ValidateName(name);
        ValidateCpf(cpf);

        Id = Guid.NewGuid();
        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
        IsActive = true;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public string Cpf { get; private set; }

    public DateTime BirthDate { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime? DeactivatedAt { get; private set; }

    public IReadOnlyCollection<PhoneNumber> PhoneNumbers =>
        _phoneNumbers.AsReadOnly();

    public void Update(string name, string cpf, DateTime birthDate)
    {
        ValidateName(name);
        ValidateCpf(cpf);

        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
    }

    public void AddPhoneNumber(
        PhoneNumberType type,
        string number)
    {
        var phoneNumber = new PhoneNumber(type, number);

        _phoneNumbers.Add(phoneNumber);
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        DeactivatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "O nome do parceiro de negócio é obrigatório.",
                nameof(name));
    }

    private static void ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException(
                "O CPF é obrigatório.",
                nameof(cpf));
    }
}