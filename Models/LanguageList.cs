namespace MyRecipes.Models
{
    using SQLite;
    using System.ComponentModel.DataAnnotations;

    public class LanguageList
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required(ErrorMessage = "Language Name is required")]
        public string LanguageName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language Code is required")]
        public string LanguageCode { get; set; } = string.Empty;

        public bool IsSelected { get; set; }
    }
}
