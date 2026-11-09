using System.Data;
using Microsoft.Data.SqlClient;

namespace SQLInjectionTest;

/// <summary>
/// Holds both safe and intentionally vulnerable SQL queries for comparison.
/// All queries connect through a restricted lab SQL login.
/// </summary>
public sealed class SQLInjectionTestRepository
{
    private readonly string _connectionString;

    public SQLInjectionTestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<SQLInjectionTestUser>> SearchUnsafeAsync(
        string name, CancellationToken cancellationToken = default)
    {
        // INTENTIONALLY INSECURE. Input becomes executable SQL, including stacked statements.
        string sql = $"SELECT Id, Name FROM [SQLInjectionTest].[Users] WHERE Name = '{name}'";

        return await ExecuteSearchAsync(sql, null, cancellationToken);
    }

    public async Task<IReadOnlyList<SQLInjectionTestUser>> SearchSafeAsync(
        string name, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, Name FROM [SQLInjectionTest].[Users] WHERE Name = @Name";

        return await ExecuteSearchAsync(sql, name, cancellationToken);
    }

    private async Task<IReadOnlyList<SQLInjectionTestUser>> ExecuteSearchAsync(
        string sql, string? nameParameter, CancellationToken cancellationToken)
    {
        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = new SqlCommand(sql, connection);

        if (nameParameter is not null)
        {
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = nameParameter;
        }

        List<SQLInjectionTestUser> users = new List<SQLInjectionTestUser>();
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        // Consume ALL result sets, including those produced by injected stacked SELECT statements.
        do
        {
            bool isUserResultSet = reader.FieldCount >= 2 &&
                string.Equals(reader.GetName(0), "Id", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(reader.GetName(1), "Name", StringComparison.OrdinalIgnoreCase);

            if (isUserResultSet is false)
            {
                continue;
            }

            while (await reader.ReadAsync(cancellationToken))
            {
                users.Add(new SQLInjectionTestUser(reader.GetInt32(0), reader.GetString(1)));
            }
        }
        while (await reader.NextResultAsync(cancellationToken));

        return users;
    }

    public async Task<bool> DisposableTableExistsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT CAST(CASE WHEN OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') IS NOT NULL THEN 1 ELSE 0 END AS bit)";

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = new SqlCommand(sql, connection);
        object? result = await command.ExecuteScalarAsync(cancellationToken);

        return result is bool exists && exists;
    }

    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        // Only recreate objects in the restricted SQLInjectionTest schema.
        const string sql = """
            SET XACT_ABORT ON;
            BEGIN TRANSACTION;

            IF OBJECT_ID(N'SQLInjectionTest.Users', N'U') IS NULL
            BEGIN
                CREATE TABLE [SQLInjectionTest].[Users]
                (
                    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
                    Name NVARCHAR(128) NOT NULL
                );
            END;

            DELETE FROM [SQLInjectionTest].[Users];
            INSERT INTO [SQLInjectionTest].[Users] (Name)
            VALUES (N'Alice'), (N'Bob'), (N'Charlie');

            IF OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') IS NULL
            BEGIN
                CREATE TABLE [SQLInjectionTest].[Disposable]
                (
                    Id INT NOT NULL PRIMARY KEY,
                    Description NVARCHAR(200) NOT NULL
                );
            END;

            DELETE FROM [SQLInjectionTest].[Disposable];
            INSERT INTO [SQLInjectionTest].[Disposable] (Id, Description)
            VALUES (1, N'This table is intentionally safe to DROP in the lab.');

            COMMIT TRANSACTION;
            """;

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

public sealed record SQLInjectionTestUser(int Id, string Name);
