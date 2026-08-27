namespace AgroSupply.BusinessPartners.Api.Contracts.BusinessRelationships;

public class CreateBusinessRelationshipRequest
{
    public Guid SupplierBusinessPartnerId { get; set; }
    public Guid BuyerBusinessPartnerId { get; set; }
}