namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;
    using System.Globalization;

    public partial class Settings
    {
        [Inject] IAppStateService AppState { get; set; }
        [Inject] ILanguageStateService LanguageStateService { get; set; }
        [Inject] ILocalizationService LocalizationService { get; set; }
        [Inject] private IVersionProvider VersionProvider { get; set; }
        private List<LanguageModel> LanguageList { get; set; } = new List<LanguageModel>();

        protected string SelectedLanguageCode { get; set; }

        private string AppVersion;
        private string Version;
        private string Build;
        private string LatestVersion;

        protected override async Task OnInitializedAsync()
        {
            AppVersion = await VersionProvider.GetVersionAsync();
            LanguageList = await GetLanguageList();
            Version = VersionProvider.Version;
            Build = VersionProvider.Build;
            StateHasChanged();
        }

        #region Language
        private async Task<List<LanguageModel>> GetLanguageList()
        {
            var languages = LocalizationService.GetLanguageList();

            var setSelectedLanguage = await SecureStorage.GetAsync("selectedLanguageCode") ?? LocalizationService.GetDefaultLanguageCode();

            foreach (var language in languages)
            {
                language.IsSelected = language.LanguageCode == setSelectedLanguage;
                if (language.IsSelected)
                {
                    SelectedLanguageCode = language.LanguageCode ?? LocalizationService.GetDefaultLanguageCode();
                }
            }

            return languages;
        }

        private async Task OnLanguageChanged(ChangeEventArgs e)
        {
            SelectedLanguageCode = e.Value?.ToString();

            var selectedLang = LanguageList.FirstOrDefault(l => l.LanguageCode == SelectedLanguageCode);

            if (selectedLang != null)
            {
                LocalizationService.SetCulture(new CultureInfo(selectedLang.LanguageCode ?? LocalizationService.GetDefaultLanguageCode()));
                await SecureStorage.SetAsync("selectedLanguageCode", SelectedLanguageCode ?? LocalizationService.GetDefaultLanguageCode());
                AppState.SelectedLanguageCode = SelectedLanguageCode ?? LocalizationService.GetDefaultLanguageCode();
            }

            //await OfflineDataService.UpdateSelectedLangaugCodeAsync(SelectedLanguageCode ?? LocalizationService.GetDefaultLanguageCode());

            LanguageStateService.NotifyLanguageChanged();

            StateHasChanged();
        }
        #endregion

        private void SaveSettings()
        {
            // Handle save
        }
    }
}