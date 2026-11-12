namespace TravelApp.BusinessLogic.Exceptions
{
    public sealed class CustomConflictException : CustomException
    {
        public CustomConflictException(string message) : base(message)
        {
        }
    }
}
