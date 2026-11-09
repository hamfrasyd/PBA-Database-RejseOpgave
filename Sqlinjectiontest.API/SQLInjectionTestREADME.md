# SQLInjectionTest – ASP.NET Core Web API + Swagger

This is the **same uploaded `Sqlinjectiontest.API` project**, not a second/new API project. It uses .NET 10, `Microsoft.Data.SqlClient` and Swashbuckle. The intentionally insecure SQL stays in `SQLInjectionTestRepository.cs`.

## Before starting

- Have **.NET 10 SDK** installed.
- Start the **local SQL Server 2022 Docker container** from your original `PBA-Database-RejseOpgave` repository. From that repository's root, run `docker compose up -d` (its `compose.yaml` maps localhost:1433).
- Run your original **TravelDb.Setup** once to create/update the `TravelDb` database. Alternatively, if `TravelDb` already exists locally, do not recreate it.
- In **SSMS**, connect to `localhost,1433` using the local Docker admin login. Execute `SQLInjectionTestSetup.sql` from this API folder. This creates only `[SQLInjectionTest].[Users]`, `[SQLInjectionTest].[Disposable]` and a dedicated restricted test login.

**Important:** The SQL file is **not a DbUp migration** and should not go into `TravelDb.Scripts`. It is a local-only training setup. Run it manually, once (safe to repeat after resetting). Do not run it on a shared or production instance.

## Connection string

The original API's `appsettings.json` had a broken `Server=Server=localhost` connection string and used `sa`. Both were corrected. The lab connection string is now in **`appsettings.Development.json`**, matching the SQL setup script exactly:

```json
"ConnectionStrings": {
  "SQLInjectionTest": "Server=localhost,1433;Database=TravelDb;User Id=SQLInjectionTestLabLogin;Password=SQLInjectionTest!2026LocalOnly;Encrypt=True;TrustServerCertificate=True;"
}
```

It uses the same local SQL Server and TravelDb database, but **a separate SQL schema**. The SQL login is only authorized to manipulate objects in that schema. The API also refuses to use a non-lab account or a nonlocal SQL Server. The bundled password is an instructional, local-only example and must not be used or committed in deployed applications.

## Start the API and use Swagger

From the extracted `Sqlinjectiontest.API` folder:

```powershell
dotnet restore
dotnet run --launch-profile http
```

Open **http://localhost:5162/swagger**. Or press F5 with the HTTP launch profile in Visual Studio. Swagger is available in Development.

Three endpoints appear under `SQLInjectionTest`:

| Method | Path | Meaning |
|---|---|---|
| GET | `/api/sql-injection-test/unsafe?name=Alice` | Builds SQL by interpolating user input (deliberately vulnerable) |
| GET | `/api/sql-injection-test/safe?name=Alice` | Runs the same search with `@Name` parameter |
| POST | `/api/sql-injection-test/reset` | Restores the demo tables after a destructive test |

In Swagger, expand an endpoint, choose **Try it out**, enter a `name` value, then **Execute**. Swagger URL-encodes the input automatically. For `reset`, just click **Execute**.

## Suggested Swagger test sequence

1. Both `/unsafe` and `/safe` with `Alice` should return one match.
2. Try `' OR 1=1 --` on `/unsafe`: all three demo users may be returned. On `/safe`, the injected text is searched as a literal and returns no matches.
3. Try `Alice'; SELECT Id, Name FROM [SQLInjectionTest].[Users]; --` on `/unsafe`: the repository processes both SQL result sets, returning Alice and the three users from the second `SELECT`. On `/safe`, nothing is injected.
4. Try `Alice'; DROP TABLE [SQLInjectionTest].[Disposable]; --` on `/unsafe`: the response should include `disposableTableExists: false`. The main TravelDb tables are not accessible to the lab login.
5. Run `POST /api/sql-injection-test/reset`: it recreates the table and reseeds Alice/Bob/Charlie.
6. Repeat the DROP input against `/safe`: it will not delete the disposable table; `disposableTableExists` remains `true`.

**Note:** The intentionally vulnerable endpoint can execute injected SQL in its restricted schema. Run this project only locally and only with disposable data. Avoid changing the SQL user to `sa`, even temporarily.

The same requests, URL-encoded, are available in `SQLInjectionTestRequests.http`. In non-Development environments the lab endpoints are hidden before controller activation.

## Structure / ownership

```text
Sqlinjectiontest.API/
  Program.cs                              # DI + Swagger + controller mapping
  Sqlinjectiontest.API.csproj             # ASP.NET Core Web API, .NET 10
  appsettings.json                        # No SQL admin credentials
  appsettings.Development.json            # Lab-only connection string
  Controllers/
    SQLInjectionTestController.cs         # GET unsafe, GET safe, POST reset
  SQLInjectionTestRepositoryContract.cs   # ISQLInjectionTestRepository
  SQLInjectionTestRepository.cs           # Raw SQL and parameter binding
  SQLInjectionTestModels.cs               # Response types used in Swagger
  SQLInjectionTestSetup.sql               # One-time local SQL setup
  SQLInjectionTestRequests.http            # Executable request examples
  SQLInjectionTestREADME.md               # This guide
```

Flow: **Swagger → SQLInjectionTestController → ISQLInjectionTestRepository → SQLInjectionTestRepository → local SQL Server → SQLInjectionTest schema.**

### Clean removal

Remove the files prefixed `SQLInjectionTest` (including the controller), delete the single `AddScoped<ISQLInjectionTestRepository>` registration in `Program.cs`, and remove the connection string from `appsettings.Development.json`. Your existing API project and Swagger infrastructure remain intact. The lab schema and login can then be deleted with the optional cleanup below, only on your local database.

```sql
USE [TravelDb];
DROP TABLE IF EXISTS [SQLInjectionTest].[Disposable], [SQLInjectionTest].[Users];
DROP SCHEMA IF EXISTS [SQLInjectionTest];
DROP USER IF EXISTS [SQLInjectionTestLabUser];
GO
USE [master];
DROP LOGIN IF EXISTS [SQLInjectionTestLabLogin];
GO
```

## Verification scope

All source paths, DI references, JSON configs and route examples were checked against the uploaded ZIP. Actual `dotnet build` and live SQL/Swagger requests could not be executed in this environment because there is no .NET SDK or SQL Server runtime here.
