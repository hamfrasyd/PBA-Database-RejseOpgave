using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SQLInjectionTest;

/// <summary>
/// Local-only teaching endpoints for SQL injection experiments.
/// Copy this file into your existing ASP.NET Core Web API project.
/// </summary>
[ApiController]
[Route("api/sql-injection-test")]
public sealed class SQLInjectionTestController : ControllerBase
{
    private readonly SQLInjectionTestRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public SQLInjectionTestController(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;

        string connectionString = configuration.GetConnectionString("SQLInjectionTest")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:SQLInjectionTest is missing. Read SQLInjectionTestREADME.md.");

        // Uses the existing API's IConfiguration. No new project or DI registration needed.
        _repository = new SQLInjectionTestRepository(connectionString);
    }

    /// <summary>
    /// Deliberately vulnerable: incorporates user input directly into SQL text.
    /// </summary>
    [HttpGet("unsafe")]
    public async Task<IActionResult> SearchUnsafeAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The 'name' query parameter is required.");
        }

        IReadOnlyList<SQLInjectionTestUser> users =
            await _repository.SearchUnsafeAsync(name, cancellationToken);

        bool disposableTableExists =
            await _repository.DisposableTableExistsAsync(cancellationToken);

        return Ok(new { Mode = "Unsafe: concatenated SQL", Users = users, DisposableTableExists = disposableTableExists });
    }

    /// <summary>
    /// Uses exactly the same query but binds the name as an SQL parameter.
    /// </summary>
    [HttpGet("safe")]
    public async Task<IActionResult> SearchSafeAsync([FromQuery] string name, CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The 'name' query parameter is required.");
        }

        IReadOnlyList<SQLInjectionTestUser> users =
            await _repository.SearchSafeAsync(name, cancellationToken);

        bool disposableTableExists =
            await _repository.DisposableTableExistsAsync(cancellationToken);

        return Ok(new { Mode = "Safe: SQL parameter", Users = users, DisposableTableExists = disposableTableExists });
    }

    /// <summary>
    /// Recreates the deliberately disposable lab tables and sample rows.
    /// </summary>
    [HttpPost("reset")]
    public async Task<IActionResult> ResetAsync(CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        await _repository.ResetAsync(cancellationToken);

        return Ok(new { Message = "SQLInjectionTest tables restored", DisposableTableExists = true });
    }

    private bool IsLocalDevelopmentRequest()
    {
        IPAddress? remoteAddress = HttpContext.Connection.RemoteIpAddress;

        return _environment.IsDevelopment() &&
            remoteAddress is not null &&
            IPAddress.IsLoopback(remoteAddress);
    }
}
