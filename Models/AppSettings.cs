namespace MyRecipes.Models
{
    public class AppSettings
    {
        public FeedbackSettings Feedback { get; set; } = new();
        public string LatestVersion { get; set; } = string.Empty;
    }

    public class FeedbackSettings
    {
        public string Email { get; set; } = string.Empty;
    }
}
