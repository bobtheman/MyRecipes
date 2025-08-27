namespace MyRecipes.Services.Interface
{
    public interface IVersionProvider
    {
        Task<string> GetVersionAsync();
        string Version { get; }
        string Build { get; }
        string LatestVersion { get; }
    }
}
