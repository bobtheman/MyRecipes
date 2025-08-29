namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;

    public partial class AddRecipe
    {
        [Inject] IAppStateService AppStateService { get; set; }
        [Inject] ILocalizationService LocalizationService { get; set; }

        private RecipeItem recipeItem = new RecipeItem();

        private EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            AppStateService.ShowSpinner = true;
            AppStateService.SelectedPage = LocalizationService["AddRecipe"];

            _editContext = new EditContext(recipeItem);

            AppStateService.ShowSpinner = false;
        }

        private void AddImage()
        {
            // TODO: Implement image picker logic
        }

        private void TakeImage()
        {
            // TODO: Implement camera capture logic
        }

        private void SaveItem()
        {
            // TODO: Handle form submission
        }
    }
}