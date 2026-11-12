namespace TravelApp.BusinessLogic.Exceptions
{
    public sealed class CustomValidationException : CustomException
    {
        public CustomValidationException(string message) : base(message)
        {
        }
    }
}
