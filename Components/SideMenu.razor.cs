namespace MyRecipes.Components
{
    using Microsoft.AspNetCore.Components;
    using MyRecipes.Services.Interface;

    public partial class SideMenu
    {
        [Parameter] public string? Class { get; set; }
        [Parameter] public EventCallback OnMenuItemClicked { get; set; }
        [Inject] ILocalizationService LocalizationService { get; set; }
    }
}