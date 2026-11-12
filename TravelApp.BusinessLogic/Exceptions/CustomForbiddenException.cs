namespace TravelApp.BusinessLogic.Exceptions
{
    public sealed class CustomForbiddenException : CustomException
    {
        public CustomForbiddenException(string message) : base(message)
        {
        }
    }
}
