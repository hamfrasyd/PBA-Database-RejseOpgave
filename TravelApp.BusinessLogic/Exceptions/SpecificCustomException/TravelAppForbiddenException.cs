using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    public sealed class TravelAppForbiddenException : TravelAppException
    {
        public TravelAppForbiddenException(string message) : base(message)
        {
        }
    }
}
