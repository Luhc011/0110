namespace Blog;

public static class Configuration
{
    public static string JwtKey { get; set; } = "KGYkBts3b0qTAGhQSLgqBw=";
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
