using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Domain.Entities;

public class BusinessRelationship
{
    private BusinessRelationship()
    {
    }

    public BusinessRelationship(
        Guid supplierBusinessPartnerId,
        Guid buyerBusinessPartnerId)
    {
        if (supplierBusinessPartnerId == Guid.Empty)
            throw new ArgumentException(
                "O parceiro fornecedor é obrigatório.",
                nameof(supplierBusinessPartnerId));

        if (buyerBusinessPartnerId == Guid.Empty)
            throw new ArgumentException(
                "O parceiro comprador é obrigatório.",
                nameof(buyerBusinessPartnerId));

        if (supplierBusinessPartnerId == buyerBusinessPartnerId)
            throw new ArgumentException(
                "Um parceiro de negócio não pode estabelecer uma relação comercial consigo mesmo.");

        Id = Guid.NewGuid();
        SupplierBusinessPartnerId = supplierBusinessPartnerId;
        BuyerBusinessPartnerId = buyerBusinessPartnerId;
        Status = BusinessRelationshipStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid SupplierBusinessPartnerId { get; private set; }

    public Guid BuyerBusinessPartnerId { get; private set; }

    public BusinessRelationshipStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? DeactivatedAt { get; private set; }

    public void Deactivate()
    {
        if (Status == BusinessRelationshipStatus.Inactive)
            return;

        Status = BusinessRelationshipStatus.Inactive;
        DeactivatedAt = DateTime.UtcNow;
    }
}