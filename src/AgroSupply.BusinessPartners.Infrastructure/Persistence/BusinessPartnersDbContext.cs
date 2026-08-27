using AgroSupply.BusinessPartners.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgroSupply.BusinessPartners.Infrastructure.Persistence;

public class BusinessPartnersDbContext : DbContext
{
    public BusinessPartnersDbContext(
        DbContextOptions<BusinessPartnersDbContext> options)
        : base(options)
    {
    }

    public DbSet<BusinessPartner> BusinessPartners { get; set; }

    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BusinessPartnersDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<BusinessRelationship> BusinessRelationships { get; set; }
}