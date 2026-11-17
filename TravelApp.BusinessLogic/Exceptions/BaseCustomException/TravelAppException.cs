namespace TravelApp.BusinessLogic.Exceptions.BaseCustomException
{
    /// <summary>Base type for application-specific exceptions.</summary>
    public abstract class TravelAppException : Exception
    {
        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="message">The error message.</param>
        protected TravelAppException(string message) : base(message)
        {
        }
    }
}
