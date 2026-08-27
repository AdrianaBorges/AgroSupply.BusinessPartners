using AgroSupply.BusinessPartners.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSupply.BusinessPartners.Infrastructure.Persistence.Configurations;

public class BusinessRelationshipConfiguration
    : IEntityTypeConfiguration<BusinessRelationship>
{
    public void Configure(
        EntityTypeBuilder<BusinessRelationship> builder)
    {
        builder.ToTable("BusinessRelationships");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SupplierBusinessPartnerId)
            .IsRequired();

        builder.Property(x => x.BuyerBusinessPartnerId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.DeactivatedAt)
            .IsRequired(false);

        builder.HasOne<BusinessPartner>()
            .WithMany()
            .HasForeignKey(x => x.SupplierBusinessPartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<BusinessPartner>()
            .WithMany()
            .HasForeignKey(x => x.BuyerBusinessPartnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SupplierBusinessPartnerId);

        builder.HasIndex(x => x.BuyerBusinessPartnerId);
    }
}