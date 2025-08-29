namespace MyRecipes.Models
{
    using MyRecipes.Services.Interface;
    using SQLite;
    using System.ComponentModel.DataAnnotations;

    public class RecipeItem
    {
        private static readonly ILocalizationService? _localizationService = ServiceLocator.GetService<ILocalizationService>();

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = nameof(NameRequiredError))]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = nameof(DescriptionLengthError))]
        public string Description { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public static string NameRequiredError => _localizationService?["NameIsRequired"] ?? "Name is required";
        public static string DescriptionLengthError => _localizationService?["DescriptionMaxLength"] ?? "Description can't exceed 100 characters";
    }
}
