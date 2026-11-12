namespace TravelApp.BusinessLogic.Exceptions.Filters
{
    /// <summary>
    /// Represents an error response returned by the API.
    /// </summary>
    public sealed class ErrorResponse
    {
        /// <summary>
        /// The required error message.
        /// </summary>
        public required string Error { get; init; }
    }
}
