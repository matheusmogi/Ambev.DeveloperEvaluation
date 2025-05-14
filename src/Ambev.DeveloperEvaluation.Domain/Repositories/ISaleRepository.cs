using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Represents a repository interface for managing sales in the system.
/// Provides methods to perform CRUD operations and query sales.
/// </summary>
public interface ISaleRepository  
{
    /// <summary>
    /// Creates a new sale asynchronously in the repository.
    /// </summary>
    /// <param name="sale">The sale entity to be created.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>The newly created sale entity.</returns>
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by its id asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>The sale entity if found; otherwise, null.</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all sales with optional pagination, sorting, and filtering parameters asynchronously.
    /// </summary>
    /// <param name="page">The page number to retrieve. Default is 1.</param>
    /// <param name="size">The number of records per page. Default is 10.</param>
    /// <param name="order">The sort order, either "asc" or "desc". Default is "asc".</param>
    /// <param name="filters">Optional filters to apply on the sales data as key-value pairs.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>A collection of sales matching the specified criteria.</returns>
    Task<IEnumerable<Sale>> GetAllAsync(int page = 1, int size = 10, string order = "asc",
        Dictionary<string, string>? filters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing sale in the repository asynchronously.
    /// </summary>
    /// <param name="sale">The sale entity with updated information.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>A boolean value indicating whether the update was successful.</returns>
    Task<bool> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale asynchronously from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to be deleted.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>A boolean value indicating whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a sale with the specified ID is active.
    /// </summary>
    /// <param name="id">The unique identifier of the sale to check.</param>
    /// <param name="cancellationToken">Cancellation token for operation cancellation.</param>
    /// <returns>A boolean value indicating whether the sale is active.</returns>
    Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default);
}