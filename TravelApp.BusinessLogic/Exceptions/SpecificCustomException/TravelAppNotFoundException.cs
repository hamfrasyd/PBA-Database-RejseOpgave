using TravelApp.BusinessLogic.Exceptions.BaseCustomException;

namespace TravelApp.BusinessLogic.Exceptions.SpecificCustomException
{
    public sealed class TravelAppNotFoundException : TravelAppException
    {
        public TravelAppNotFoundException(string message) : base(message)
        {
        }
    }
}
