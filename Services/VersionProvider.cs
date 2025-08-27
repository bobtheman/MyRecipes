namespace MyRecipes.Services
{
    using Microsoft.Extensions.Options;
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;
    using System.Reflection;

    public class VersionProvider : IVersionProvider
    {
        private readonly IOptions<AppSettings> AppSettingsOptions;

        public VersionProvider(IOptions<AppSettings> appSettingsOptions)
        {
            AppSettingsOptions = appSettingsOptions;
        }

        public Task<string> GetVersionAsync()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            return Task.FromResult(version);
        }

        public string Version => AppInfo.VersionString;
        public string Build => AppInfo.BuildString;
        public string LatestVersion => AppSettingsOptions.Value.LatestVersion;
    }
}
