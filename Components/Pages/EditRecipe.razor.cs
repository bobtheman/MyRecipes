namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using MyRecipes.Services.Interface;

    public partial class EditRecipe
    {
        [Inject] ILocalizationService LocalizationService { get; set; }

        protected override async Task OnInitializedAsync()
        {

        }
    }
}