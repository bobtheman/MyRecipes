namespace MyRecipes.Services.Interface
{
    using MyRecipes.Models;

    public interface IOfflineDataService
    {
        Task InitializeAsync();
        Task AddItemAsync(RecipeItem recipeItem);
        Task UpdateItemAsync(RecipeItem recipeItem);
        Task DeleteItemAsync(int id);
        Task CheckSelectedLanguageAsync();
        Task<RecipeList> GetAllRecipeListPagedAsync(int skip, int take);
    }
}
