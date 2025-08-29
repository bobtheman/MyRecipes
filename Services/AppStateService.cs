namespace MyRecipes.Services
{
    using MyRecipes.Services.Interface;

    public class AppStateService: IAppStateService
    {
        public event Action? OnChange;

        private string _selectedPage = string.Empty;
        public string SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
        }

        private bool _showSpinner = false;
        public bool ShowSpinner
        {
            get => _showSpinner;
            set => SetBoolProperty(ref _showSpinner, value);
        }

        private string _selectedLanguageCode = string.Empty;
        public string SelectedLanguageCode
        {
            get => _selectedLanguageCode;
            set => SetProperty(ref _selectedLanguageCode, value);
        }

        private void SetProperty(ref string field, string value)
        {
            if (field != value)
            {
                field = value;
                NotifyStateChanged();
            }
        }

        private void SetBoolProperty(ref bool field, bool value)
        {
            if (field != value)
            {
                field = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
    }
}
