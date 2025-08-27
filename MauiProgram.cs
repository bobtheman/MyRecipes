using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyRecipes.Models;
using MyRecipes.Services;
using MyRecipes.Services.Interface;
using System.Reflection;

namespace MyRecipes
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Load embedded appsettings.json
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("MyRecipes.appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream!)
                .Build();

            // Add config to the DI container
            builder.Configuration.AddConfiguration(config);

            var appSettingsSection = config.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(options =>
            {
                appSettingsSection.Bind(options);
            });

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // Register services
            builder.Services.AddScoped<IAppStateService, AppStateService > ();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ILanguageStateService, LanguageStateService > ();
            builder.Services.AddScoped<ILocalizationService, LocalizationService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IVersionProvider, VersionProvider>();

            return builder.Build();
        }
    }
}
