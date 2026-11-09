namespace Sqlinjectiontest.API;

/// <summary>
/// SQL injection lab queries and test-data reset operations.
/// </summary>
public interface ISQLInjectionTestRepository
{
    Task<IReadOnlyList<SQLInjectionTestUser>> SearchUnsafeAsync(
        string name, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SQLInjectionTestUser>> SearchSafeAsync(
        string name, CancellationToken cancellationToken = default);

    Task<bool> DisposableTableExistsAsync(
        CancellationToken cancellationToken = default);

    Task ResetAsync(CancellationToken cancellationToken = default);
}
