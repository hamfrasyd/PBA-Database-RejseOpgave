using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    public sealed class TravelAppConflictException : TravelAppException
    {
        public TravelAppConflictException(string message) : base(message)
        {
        }
    }
}
