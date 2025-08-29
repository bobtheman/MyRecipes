namespace MyRecipes.Models
{
    using SQLite;

    public class RecipeList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public List<RecipeItem> RecipeItem { get; set; }

        public int TotalCount { get; set; }
    }
}
