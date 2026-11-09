using System.Net;
using Microsoft.AspNetCore.Mvc;
using Sqlinjectiontest.API;

namespace Sqlinjectiontest.API.Controllers;

/// <summary>
/// Demonstrates SQL injection using a restricted, disposable local database schema.
/// </summary>
[ApiController]
[Route("api/sql-injection-test")]
public sealed class SQLInjectionTestController : ControllerBase
{
    private readonly ISQLInjectionTestRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public SQLInjectionTestController(
        ISQLInjectionTestRepository repository,
        IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    /// <summary>
    /// Searches the lab users using intentionally vulnerable SQL string interpolation.
    /// </summary>
    /// <remarks>For example, try the name: ' OR 1=1 --</remarks>
    [HttpGet("unsafe")]
    [ProducesResponseType(typeof(SQLInjectionTestSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SQLInjectionTestSearchResponse>> SearchUnsafeAsync(
        [FromQuery] string name, CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The 'name' parameter is required.");
        }

        if (name.Length > 128)
        {
            return BadRequest("The 'name' parameter cannot exceed 128 characters.");
        }

        IReadOnlyList<SQLInjectionTestUser> users =
            await _repository.SearchUnsafeAsync(name, cancellationToken);

        bool disposableTableExists =
            await _repository.DisposableTableExistsAsync(cancellationToken);

        return Ok(new SQLInjectionTestSearchResponse(
            "Unsafe (SQL interpolation)", users, disposableTableExists));
    }

    /// <summary>
    /// Searches the same lab users using a parameterized SQL query.
    /// </summary>
    [HttpGet("safe")]
    [ProducesResponseType(typeof(SQLInjectionTestSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SQLInjectionTestSearchResponse>> SearchSafeAsync(
        [FromQuery] string name, CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("The 'name' parameter is required.");
        }

        if (name.Length > 128)
        {
            return BadRequest("The 'name' parameter cannot exceed 128 characters.");
        }

        IReadOnlyList<SQLInjectionTestUser> users =
            await _repository.SearchSafeAsync(name, cancellationToken);

        bool disposableTableExists =
            await _repository.DisposableTableExistsAsync(cancellationToken);

        return Ok(new SQLInjectionTestSearchResponse(
            "Safe (SQL parameter)", users, disposableTableExists));
    }

    /// <summary>
    /// Recreates the lab tables and sample records after a destructive experiment.
    /// </summary>
    [HttpPost("reset")]
    [ProducesResponseType(typeof(SQLInjectionTestResetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SQLInjectionTestResetResponse>> ResetAsync(
        CancellationToken cancellationToken)
    {
        if (IsLocalDevelopmentRequest() is false)
        {
            return NotFound();
        }

        await _repository.ResetAsync(cancellationToken);

        return Ok(new SQLInjectionTestResetResponse(
            "Lab tables restored with Alice, Bob and Charlie.", true));
    }

    private bool IsLocalDevelopmentRequest()
    {
        IPAddress? remoteAddress = HttpContext.Connection.RemoteIpAddress;

        return _environment.IsDevelopment() &&
            remoteAddress is not null &&
            IPAddress.IsLoopback(remoteAddress);
    }
}
