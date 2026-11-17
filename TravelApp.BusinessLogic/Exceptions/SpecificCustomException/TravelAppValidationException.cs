using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    /// <summary>Thrown when a request fails validation.</summary>
    public sealed class TravelAppValidationException : TravelAppException
    {
        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="errorMessage">The error message.</param>
        public TravelAppValidationException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
