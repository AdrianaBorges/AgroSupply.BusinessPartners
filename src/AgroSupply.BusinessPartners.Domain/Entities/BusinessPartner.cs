namespace AgroSupply.BusinessPartners.Domain.Entities;

public class BusinessPartner
{
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

    public void Update(string name, string cpf, DateTime birthDate)
    {
        ValidateName(name);
        ValidateCpf(cpf);

        Name = name;
        Cpf = cpf;
        BirthDate = birthDate;
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