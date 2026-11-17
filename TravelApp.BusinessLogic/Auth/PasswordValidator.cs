namespace TravelApp.BusinessLogic.Auth
{
    /// <summary>Validates passwords when accounts are created or updated.</summary>
    public static class PasswordValidator
    {
        /// <summary>Checks whether the password meets the length requirement.</summary>
        public static bool IsValid(string? password)
        {
            return password is { Length: >= 15 and <= 64 };
        }
    }
}