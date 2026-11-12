using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    public sealed class TravelAppValidationException : TravelAppException
    {
        public TravelAppValidationException(string message) : base(message)
        {
        }
    }
}
