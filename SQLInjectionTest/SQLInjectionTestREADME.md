# SQLInjectionTest – drop-in files for your EXISTING API

## Important: no API project was present in the uploaded ZIP

The original `PBA-Database-RejseOpgave-master.zip` only contains `TravelDb.Setup` (console app), `TravelDb.Scripts`, and related files. It contains **no** ASP.NET Core Web API project or existing controllers.

Therefore these files were added in the **SQLInjectionTest/** folder, NOT wired into a new project. No existing `.csproj`, `.slnx`, migration, or `Program.cs` was changed.

## Set up (local machine only)

1. Run the original `TravelDb.Setup` normally to create the local `TravelDb` database.
2. In SSMS, connect to **your local SQL Server** using an administrative account and run `SQLInjectionTestSetup.sql` manually. It creates `SQLInjectionTest.Users`, `SQLInjectionTest.Disposable`, and a login with rights only for the test schema. **Do not put this .sql file inside TravelDb.Scripts**: DbUp otherwise runs it automatically.
3. Copy `SQLInjectionTestController.cs` and `SQLInjectionTestRepository.cs` into your **existing ASP.NET Core API project** (any folder, e.g. `Controllers/` and `Repositories/`). The namespace is deliberately self-contained: `SQLInjectionTest`.
4. Make sure the API project references `Microsoft.Data.SqlClient`. If it does not, run `dotnet add <path-to-existing-api.csproj> package Microsoft.Data.SqlClient`.
5. Add this **separate lab-only** connection string to your existing API's `appsettings.Development.json` under `ConnectionStrings`:

   ```json
   {
     "ConnectionStrings": {
       "SQLInjectionTest": "Server=localhost,1433;Database=TravelDb;User Id=SQLInjectionTestLabLogin;Password=SQLInjectionTest!2026LocalOnly;Encrypt=False;TrustServerCertificate=True;"
     }
   }
   ```

   **Merge into the existing JSON.** Do not overwrite existing settings. Change server/port if required. Change the example password if the lab is not strictly isolated. Never commit the sample secret to a public repository.

6. Run **the existing API** using its usual launch profile. Its `Program.cs` must already call `AddControllers()` and `MapControllers()`. If these are not present, enable standard controller routing. No new API project or standalone `Program.cs` is required.
7. The endpoints work **only** with `ASPNETCORE_ENVIRONMENT=Development` and requests arriving from localhost. Use Postman against the port of your **existing** API.

## Endpoints

| Method | Route | Behavior |
|---|---|---|
| GET | `/api/sql-injection-test/unsafe?name=Alice` | Intentionally vulnerable, string concatenation |
| GET | `/api/sql-injection-test/safe?name=Alice` | Same query with a SQL parameter |
| POST | `/api/sql-injection-test/reset` | Recreate lab tables / seed rows |

### Examples for `name` (Postman Params tab URL-encodes them)

Normal search:

```text
Alice
```

Bypass the WHERE condition:

```text
' OR 1=1 --
```

Stack two SELECT statements (the second result set is also read):

```text
Alice'; SELECT Id, Name FROM [SQLInjectionTest].[Users]; --
```

Execute a SELECT and then DROP the **disposable lab table only**:

```text
Alice'; DROP TABLE [SQLInjectionTest].[Disposable]; --
```

The unsafe request should return `DisposableTableExists: false` after the DROP. The parameterized endpoint should not execute these strings as SQL. Call `POST /api/sql-injection-test/reset` afterward to restore the table.

### Why use a restricted login?

The unsafe query is **genuinely injectable**. It is not sufficient to tell users to only DROP a specific table: arbitrary injected statements could target other tables if the SQL login has permission. The lab login only receives database CONNECT, CREATE TABLE, and CONTROL permissions on the `SQLInjectionTest` schema. Do **not** replace it with `sa`, dbo, or your normal privileged TravelDb login. Do not expose the endpoints beyond local development.

### Removal

Delete `SQLInjectionTestController.cs`, `SQLInjectionTestRepository.cs`, and the `SQLInjectionTest` connection-string entry from your existing API, then delete the top-level `SQLInjectionTest/` folder from this repository. The main application code is unchanged.

Optional SQL cleanup **after you have finished**:

```sql
USE [TravelDb];
DROP TABLE IF EXISTS [SQLInjectionTest].[Users], [SQLInjectionTest].[Disposable];
DROP SCHEMA IF EXISTS [SQLInjectionTest];
DROP USER IF EXISTS [SQLInjectionTestLabUser];
GO
USE [master];
DROP LOGIN IF EXISTS [SQLInjectionTestLabLogin];
```

Do not run the cleanup while you still need the lab.

### Validation status

The ZIP content and exact original-file preservation were verified. The C# could not be compiled or tested with SQL Server in this environment because .NET SDK and SQL Server/Docker are not installed here.
