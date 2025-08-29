namespace MyRecipes.Services.Interface
{
    public interface IAppStateService
    {
        event Action? OnChange;
        string SelectedPage { get; set; }
        bool ShowSpinner { get; set; }
        string SelectedLanguageCode { get; set; }
    }
}
