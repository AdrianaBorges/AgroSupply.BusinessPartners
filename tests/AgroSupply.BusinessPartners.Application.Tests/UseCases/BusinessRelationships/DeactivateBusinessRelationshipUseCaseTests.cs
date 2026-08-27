using AgroSupply.BusinessPartners.Application.Abstractions;
using AgroSupply.BusinessPartners.Application.UseCases.BusinessRelationships;
using AgroSupply.BusinessPartners.Domain.Entities;
using AgroSupply.BusinessPartners.Domain.Enums;

namespace AgroSupply.BusinessPartners.Application.Tests.UseCases.BusinessRelationships;

public class DeactivateBusinessRelationshipUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldDeactivateRelationship_WhenRelationshipExists()
    {
        // Arrange
        var relationship = new BusinessRelationship(
            Guid.NewGuid(),
            Guid.NewGuid());

        var repository =
            new FakeBusinessRelationshipRepository(relationship);

        var useCase =
            new DeactivateBusinessRelationshipUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(relationship.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(
            BusinessRelationshipStatus.Inactive,
            relationship.Status);
        Assert.NotNull(relationship.DeactivatedAt);
        Assert.Same(
            relationship,
            repository.UpdatedRelationship);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenRelationshipDoesNotExist()
    {
        // Arrange
        var repository =
            new FakeBusinessRelationshipRepository();

        var useCase =
            new DeactivateBusinessRelationshipUseCase(repository);

        // Act
        var result = await useCase.ExecuteAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
        Assert.Null(repository.UpdatedRelationship);
    }

    private sealed class FakeBusinessRelationshipRepository
        : IBusinessRelationshipRepository
    {
        private readonly BusinessRelationship? _relationship;

        public FakeBusinessRelationshipRepository(
            BusinessRelationship? relationship = null)
        {
            _relationship = relationship;
        }

        public BusinessRelationship? UpdatedRelationship
        {
            get;
            private set;
        }

        public Task AddAsync(
            BusinessRelationship relationship) =>
            Task.CompletedTask;

        public Task<BusinessRelationship?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                _relationship?.Id == id
                    ? _relationship
                    : null);
        }

        public Task UpdateAsync(
            BusinessRelationship relationship)
        {
            UpdatedRelationship = relationship;

            return Task.CompletedTask;
        }
    }
}