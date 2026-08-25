namespace AgroSupply.BusinessPartners.Domain.Entities;

public class BusinessPartner
{
    public BusinessPartner(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "O nome do parceiro de negócio é obrigatório.",
                nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }
}