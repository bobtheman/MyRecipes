using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyRecipes.Models;
using MyRecipes.Services;
using MyRecipes.Services.Interface;
using System.Reflection;
using CommunityToolkit.Maui;
using ZXing.Net.Maui.Controls;

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
                .UseMauiCommunityToolkit()
                .UseBarcodeReader()
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
            builder.Services.AddScoped<IAppStateService, AppStateService> ();
            builder.Services.AddScoped<IAlertService, AlertService>();
            builder.Services.AddScoped<ICameraService, CameraService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ILanguageStateService, LanguageStateService>();
            builder.Services.AddScoped<ILocalizationService, LocalizationService>();
            builder.Services.AddScoped<IOfflineDataService, OfflineDataService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IVersionProvider, VersionProvider>();

            return builder.Build();
        }
    }
}
