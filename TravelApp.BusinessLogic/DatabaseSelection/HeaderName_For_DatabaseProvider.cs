namespace TravelApp.BusinessLogic.DatabaseSelection
{
	/// <summary>
	/// Provides the database provider header name.
	/// </summary>
	/// <remarks>The default header name is "X-Database-Provider". Setting <see cref="Value"/> to null, empty, or
	/// whitespace throws an <see cref="ArgumentException"/>.</remarks>
    public static class HeaderName_For_DatabaseProvider
    {
		/// <summary>
		/// Default Constant value "x-Database-Provider"
		/// </summary>
		public const string Key = "X-Database-Provider";
    }
}
