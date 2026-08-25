namespace AgroSupply.BusinessPartners.Domain.Entities;

public class BusinessPartner
{
    public BusinessPartner(string name, string cpf, DateTime birthDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "O nome do parceiro de negócio é obrigatório.",
                nameof(name));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException(
                "O CPF é obrigatório.",
                nameof(cpf));

        Id = Guid.NewGuid();
        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
        IsActive = true;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string Cpf { get; }

    public DateTime BirthDate { get; }

    public bool IsActive { get; }
}