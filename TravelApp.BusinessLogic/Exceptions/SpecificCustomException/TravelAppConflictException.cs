using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    /// <summary>Thrown when an operation conflicts with the current state.</summary>
    public sealed class TravelAppConflictException : TravelAppException
    {
        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="errorMessage">The error message.</param>
        public TravelAppConflictException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
