using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    /// <summary>Thrown when the current user is not allowed to perform an operation.</summary>
    public sealed class TravelAppForbiddenException : TravelAppException
    {
        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="errorMessage">The error message.</param>
        public TravelAppForbiddenException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
