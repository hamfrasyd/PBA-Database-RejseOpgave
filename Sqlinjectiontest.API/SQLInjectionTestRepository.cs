using System.Data;
using Microsoft.Data.SqlClient;

namespace Sqlinjectiontest.API;

/// <summary>
/// Compares unsafe SQL interpolation with parameter binding using the same query.
/// IMPORTANT: The intentionally unsafe method is for local training only.
/// </summary>
public sealed class SQLInjectionTestRepository : ISQLInjectionTestRepository
{
    private readonly string _connectionString;

    public SQLInjectionTestRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IReadOnlyList<SQLInjectionTestUser>> SearchUnsafeAsync(
        string name, CancellationToken cancellationToken = default)
    {
        // VULNERABLE BY DESIGN: input becomes part of the executable SQL batch.
        // A crafted input can add more SELECTs or DROP lab tables.
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

        // SQL Server allows multiple statements in the same batch.
        // Read every matching result set to demonstrate stacked SELECT attacks.
        do
        {
            bool isUserResultSet = reader.FieldCount >= 2 &&
                string.Equals(reader.GetName(0), "Id", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(reader.GetName(1), "Name", StringComparison.OrdinalIgnoreCase);

            if (isUserResultSet)
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    users.Add(new SQLInjectionTestUser(
                        reader.GetInt32(0), reader.GetString(1)));
                }
            }
        }
        while (await reader.NextResultAsync(cancellationToken));

        return users;
    }

    public async Task<bool> DisposableTableExistsAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CAST(
                CASE WHEN OBJECT_ID(N'SQLInjectionTest.Disposable', N'U') IS NOT NULL
                THEN 1 ELSE 0 END AS bit)
            """;

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = new SqlCommand(sql, connection);
        object? result = await command.ExecuteScalarAsync(cancellationToken);

        return result is bool exists && exists;
    }

    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        // Only recreates tables inside the SQLInjectionTest schema.
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
            VALUES (1, N'This lab table may be dropped and recreated.');

            COMMIT TRANSACTION;
            """;

        await using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
