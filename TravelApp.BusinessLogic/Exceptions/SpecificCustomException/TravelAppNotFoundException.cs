using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    /// <summary>Thrown when a requested resource does not exist.</summary>
    public sealed class TravelAppNotFoundException : TravelAppException
    {
        /// <summary>Initializes a new instance with the specified message.</summary>
        /// <param name="errorMessage">The error message.</param>
        public TravelAppNotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
