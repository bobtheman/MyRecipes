namespace MyRecipes.Services.Interface
{
    public interface IAppStateService
    {
        event Action? OnChange;
        bool ShowSpinner { get; set; }
        string SelectedLanguageCode { get; set; }
    }
}
