namespace TravelApp.API.DatabaseSelection
{
    /// <summary>Marks endpoints that always use SQL Server.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class UseSqlDatabaseAttribute : Attribute
    {
    }
}