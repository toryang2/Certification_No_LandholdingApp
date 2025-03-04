namespace CommonLibrary
{
    public static class UserSession
    {
        public static string Username { get; set; }  // MUST be public
        public static string Initials { get; set; }  // MUST be public
        public static int AccessLevel { get; set; } = -1;  // Default to invalid
    }
}
