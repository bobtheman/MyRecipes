namespace MyRecipes.Services.Interface
{
    using MyRecipes.Models;
    using System.ComponentModel;
    using System.Globalization;
    public interface ILocalizationService : INotifyPropertyChanged
    {
        string this[string key] { get; }

        CultureInfo GetCulture();

        void SetCulture(CultureInfo culture);

        string GetDefaultLanguageCode();

        List<LanguageModel> GetLanguageList();
    }
}
