namespace TravelApp.BusinessLogic.Exceptions.BaseCustomException
{
    public abstract class TravelAppException : Exception
    {
        protected TravelAppException(string message) : base(message)
        {
        }
    }
}