namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;


    public partial class Recipes
    {
        [Inject] private IAppStateService AppStateService { get; set; }

        [Inject] private IAlertService AlertService { get; set; }

        [Inject] ILocalizationService LocalizationService { get; set; }

        [Inject] private IOfflineDataService OfflineDataService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        private RecipeList recipeList = new RecipeList();

        private int currentPage = 1;
        private int pageSize = 4;
        private int totalItems = 0;
        private int totalPages = 1;

        protected override async Task OnInitializedAsync()
        {
            if (AppStateService is null || OfflineDataService is null || LocalizationService is null)
            {
                return;
            }

            AppStateService.ShowSpinner = true;

            try
            {
                await OfflineDataService.InitializeAsync();
                await LoadPageAsync(1);
                AppStateService.SelectedPage = string.Empty;
            }
            catch (Exception ex)
            {
                await AlertService?.ShowErrorAlertAsync(LocalizationService["Error"], ex.Message);
            }
            finally
            {
                AppStateService.ShowSpinner = false;
            }
        }

        #region Pagination
        private async Task LoadPageAsync(int page)
        {
            if (OfflineDataService is null || AppStateService is null)
                return;

            AppStateService.ShowSpinner = true;

            try
            {
                currentPage = page;
                int skip = (currentPage - 1) * pageSize;

                recipeList = await OfflineDataService.GetAllRecipeListPagedAsync(skip, pageSize);
                totalItems = recipeList.RecipeItem.Count;
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            }
            catch (Exception ex)
            {
                await AlertService?.ShowErrorAlertAsync(LocalizationService["Error"], ex.Message);
            }
            finally
            {
                AppStateService.ShowSpinner = false;
                StateHasChanged();
            }
        }
        private async Task AddNew()
        {
            if (AppStateService is null)
            {
                return;
            }

            AppStateService.SelectedPage = "AddRecipe";
            NavigationManager.NavigateTo("/addrecipe");
        }
        #endregion
    }
}