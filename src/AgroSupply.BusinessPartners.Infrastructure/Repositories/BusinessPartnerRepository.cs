using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgroSupply.BusinessPartners.Infrastructure.Repositories;

public class BusinessPartnerRepository : IBusinessPartnerRepository
{
    private readonly BusinessPartnersDbContext _context;

    public BusinessPartnerRepository(BusinessPartnersDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BusinessPartner businessPartner)
    {
        await _context.BusinessPartners.AddAsync(businessPartner);
        await _context.SaveChangesAsync();
    }

    public async Task<BusinessPartner?> GetByIdAsync(Guid id)
    {
        return await _context.BusinessPartners
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyCollection<BusinessPartner>> GetAllAsync()
    {
        return await _context.BusinessPartners
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(BusinessPartner businessPartner)
    {
        _context.BusinessPartners.Update(businessPartner);
        await _context.SaveChangesAsync();
    }
}