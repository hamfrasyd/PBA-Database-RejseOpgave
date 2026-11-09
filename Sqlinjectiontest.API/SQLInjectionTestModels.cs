namespace Sqlinjectiontest.API;

public sealed record SQLInjectionTestUser(int Id, string Name);

public sealed record SQLInjectionTestSearchResponse(
    string Mode,
    IReadOnlyList<SQLInjectionTestUser> Users,
    bool DisposableTableExists);

public sealed record SQLInjectionTestResetResponse(
    string Message,
    bool DisposableTableExists);
