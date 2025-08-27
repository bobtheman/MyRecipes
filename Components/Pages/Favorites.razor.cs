namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using MyRecipes.Services.Interface;

    public partial class Favorites
    {
        [Inject] ILocalizationService LocalizationService { get; set; }
    }
}