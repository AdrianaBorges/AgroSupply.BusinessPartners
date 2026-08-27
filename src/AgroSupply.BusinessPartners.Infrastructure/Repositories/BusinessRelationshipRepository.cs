using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgroSupply.BusinessPartners.Infrastructure.Repositories;

public class BusinessRelationshipRepository
    : IBusinessRelationshipRepository
{
    private readonly BusinessPartnersDbContext _context;

    public BusinessRelationshipRepository(
        BusinessPartnersDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        BusinessRelationship relationship)
    {
        await _context.BusinessRelationships
            .AddAsync(relationship);

        await _context.SaveChangesAsync();
    }

    public async Task<BusinessRelationship?> GetByIdAsync(
        Guid id)
    {
        return await _context.BusinessRelationships
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(
        BusinessRelationship relationship)
    {
        _context.BusinessRelationships.Update(relationship);

        await _context.SaveChangesAsync();
    }
}