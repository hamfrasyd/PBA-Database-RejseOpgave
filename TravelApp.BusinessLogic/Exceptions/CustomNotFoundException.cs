namespace TravelApp.BusinessLogic.Exceptions
{
    public sealed class CustomNotFoundException : CustomException
    {
        public CustomNotFoundException(string message) : base(message)
        {
        }
    }
}
