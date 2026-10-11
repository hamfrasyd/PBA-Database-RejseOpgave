# TravelDb - PBA Database Assignment

This repository contains the database schema, procedures, functions, triggers, views, scheduled jobs and sample data, plus a small .NET console app to set everything up.

## Run the project

**Requirements:** .NET 10 SDK, Docker Desktop (or Docker Engine with Compose), and an available local port `1433`.

From the repository root, run:

```bash
dotnet run --project TravelDb.Setup/TravelDb.Setup.csproj
```

The console app will:

1. Start SQL Server 2022 using `compose.yaml` (`docker compose up -d`).
2. Wait until SQL Server accepts connections.
3. Create the `TravelDb` database if it does not exist.
4. Run any pending SQL scripts through **DbUp**, including the initial seed and test data.

The default development server is `localhost,1433`. Docker stores the database in the named `travel-mssql-data` volume, so running the app again **does not reset the data**.

> The Docker configuration is intended for local development. Do not reuse its hardcoded development credentials for a deployed database.

## Where is the SQL?

All individual scripts live in `TravelDb.Scripts/`:

```text
TravelDb.Scripts/
├── 01_Setup/             Tables and user-defined types
├── 02_Constrains/        Nullability, keys and other constraints
├── 03_StoredProcedures/  Database operations
├── 04_StoredFunctions/   Reusable database functions
├── 05_Triggers/          Automatic reactions to data changes
├── 06_Views/             Readable queries for common use cases
├── 07_Indexes/           Query performance indexes
├── 08_Events/            SQL Server Agent jobs
└── 09_InsertData/        Reference data and sample/test data
```

Read the folders **in numerical order**: create the structure first, apply its rules, add the database logic, and insert data last. Scripts in `09_InsertData/` are for data, not for defining tables, constraints or other schema objects.

### Alternative: one SQL file

[`docs/One-Full-DB-Schema.sql`](docs/One-Full-DB-Schema.sql) contains the combined, executable database setup, including seed data.

To use it instead of the console app, connect to SQL Server in **SQL Server Management Studio** or another SQL editor that supports `GO`, open the file and execute it. The script creates `TravelDb` if needed and then sets it up. SQL Server Agent support and suitable permissions are required for its scheduled job.

**Choose one setup method for a fresh database.** The combined SQL file does **not** populate DbUp's `SchemaVersions` history. Running DbUp afterward against that same database would make DbUp try to create objects that already exist. The combined file is also not intended as a repeatable reset script.

## Using a different database or SQL Server

The **current** `Program.cs` builds its connection strings directly in code.

For a **different database name on the same local SQL Server**:

1. Change `const string databaseName = "TravelDb";` in `TravelDb.Setup/Program.cs`. Both the database creation and migration connection string use this variable.
2. Change `@database_name = N'TravelDb'` in `TravelDb.Scripts/08_Events/001_ExpireInvitationsJob.sql` to the new name. If multiple databases share one SQL Server instance, also give the Agent job and schedule unique names; those names belong to the server, not to an individual database.
3. Run the console app again. DbUp will use the `SchemaVersions` table **in the new database**.

For a **different SQL Server** (for example, a remote instance), also update `masterConnectionString` and `travelDbConnectionString` in `Program.cs` with the correct server, credentials and encryption settings. The current app **always starts local Docker**, so remove or make the `DockerService.StartAsync()` call optional when targeting an existing external server. The login must have the permissions needed for database creation and migrations; the Agent job additionally requires SQL Server Agent and appropriate permissions.

If you use `docs/One-Full-DB-Schema.sql` manually instead, update its hardcoded `TravelDb` references (`CREATE DATABASE` / `USE` / database-existence check and Agent job database), plus the Agent job/schedule names if needed.

## How DbUp and `SchemaVersions` work

`TravelDb.Setup/MigrationService.cs` configures DbUp to load **all `.sql` files** from `TravelDb.Scripts/` as embedded resources, execute pending scripts in script-name order, and record successful executions in **`dbo.SchemaVersions`** in the target database.

```sql
SELECT ScriptName, Applied
FROM dbo.SchemaVersions
ORDER BY Applied, ScriptName;
```

The key rule is that **DbUp tracks executed script names, not changes to their contents**:

| What you do | What happens on the next run |
| --- | --- |
| **Add** a new `.sql` file | DbUp runs it once, then records its name in `SchemaVersions`. |
| **Edit** an already executed file | DbUp does **not** rerun it. A fresh database will use the edited version, but existing databases will not. |
| **Rename or move** a file | Its embedded script name changes, so DbUp can treat it as a **new script** and run it again. |
| **Delete** a file | DbUp does **not** undo what it previously created or inserted. The old journal entry remains, and fresh setups may break if the deleted file was needed. |

### Working with migrations safely

- **Keep already-applied scripts unchanged.** For a change to an existing table, view, procedure or seed rule, add a **new migration** instead of editing the executed one. For example, `TravelDb.Scripts/10_Migrations/001_UpdateTripOverview.sql` could contain a new `CREATE OR ALTER VIEW` statement.
- **Give new scripts unique, descriptive names** and check their execution order and dependencies. A new `.sql` file under `TravelDb.Scripts/` is included automatically by the `.csproj` embedded-resource pattern when the app is rebuilt.
- **Do not delete or rename historical migration files** just to tidy up the folder. Removing a database object requires an explicit new migration (for example, a `DROP` statement); deleting its old script does not remove the object.
- **Do not manually edit or clear `SchemaVersions`** as a normal migration workflow. Doing so can make DbUp replay operations that were already applied.
- **Treat seed scripts the same way:** DbUp runs them only once per database. If existing reference data needs correcting, create a separate data migration; if you want more test data, create a deliberate new seed script.

If a migration fails, the console app reports the error and stops. Earlier successfully applied scripts are not automatically rolled back as a group, so fix the cause and run the app again after checking the database state.

**In short:** the numbered folders describe the initial database setup; new migration files describe later changes. `SchemaVersions` tells DbUp which of those changes have already been applied to each database.
