namespace MyRecipes.Services
{
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;
    using SQLite;
    using System.Diagnostics;

    public class OfflineDataService : IOfflineDataService
    {
        private const string DbName = "MyRecipesDatabase.db3";
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
        private SQLiteAsyncConnection db;
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalItems = 0;
        private int totalPages = 1;
        private const int CurrentDbVersion = 1; // <--- Increment this when you change schema

        public OfflineDataService()
        {
            dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
            db = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Run migrations first
                int savedVersion = Preferences.Get("DbSchemaVersion", 0);
                if (savedVersion < CurrentDbVersion)
                {
                    await MigrateDatabaseAsync(savedVersion, CurrentDbVersion);
                    Preferences.Set("DbSchemaVersion", CurrentDbVersion);
                }

                await SafeCreateTableAsync<RecipeList>();
                await SafeCreateTableAsync<RecipeItem>();
                await SafeCreateTableAsync<LanguageList>();

                await CheckSelectedLanguageAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[InitializeAsync] Error: " + ex);
            }
        }

        public async Task AddItemAsync(RecipeItem recipeItem)
        {
            try
            {
                if (db == null)
                {
                    return;
                }

                //await db.InsertAsync(recipeItem);

                var result = await db.InsertAsync(recipeItem);
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine("[AddItemAsync] SQLite error: " + sqliteEx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AddItemAsync] Unexpected error: " + ex);
            }
        }

        public async Task UpdateItemAsync(RecipeItem recipeItem)
        {
            try
            {
                if (db == null)
                {
                    return;
                }
                await db.UpdateAsync(recipeItem);
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine("[UpdateItemAsync] SQLite error: " + sqliteEx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[UpdateItemAsync] Unexpected error: " + ex);
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            try
            {
                if (db == null)
                {
                    return;
                }
                await db.DeleteAsync<RecipeItem>(id);
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine("[DeleteItemAsync] SQLite error: " + sqliteEx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DeleteItemAsync] Unexpected error: " + ex);
            }
        }

        public async Task CheckSelectedLanguageAsync()
        {
            try
            {
                if (db == null)
                {
                    return;
                }

                var count = await db.Table<LanguageList>().CountAsync();
                if (count == 0)
                {
                    var english = new LanguageList
                    {
                        LanguageName = "English",
                        LanguageCode = "en-GB",
                        IsSelected = true
                    };
                    var german = new LanguageList
                    {
                        LanguageName = "German",
                        LanguageCode = "de-DE",
                        IsSelected = false
                    };

                    await db.InsertAsync(english);
                    await db.InsertAsync(german);
                }
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine("[CheckSelectedLanguageAsync] SQLite error: " + sqliteEx.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CheckSelectedLanguageAsync] Unexpected error: " + ex);
            }
        }

        public async Task<RecipeList> GetAllRecipeListPagedAsync(int skip, int take)
        {
            try
            {
                if (db == null)
                {
                    return new RecipeList();
                }

                var recipeList = new RecipeList();


                //var recipeList = await db.Table<RecipeList>()
                //                         .Skip(skip)
                //                         .Take(take)
                //                         .ToListAsync();

                //foreach (var recipe in recipeList)
                //{
                // Select all usages of: await db.Table<RecipeItem>()
                // Example usage (uncomment and adapt as needed):

                recipeList.RecipeItem = await db.Table<RecipeItem>().ToListAsync();
                //                                   .Where(x => x.Id == recipe.Id)
                //                                   .ToListAsync();
                //}

                return recipeList;
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine("[GetAllRecipeListPagedAsync] SQLite error: " + sqliteEx.Message);
                return new RecipeList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GetAllRecipeListPagedAsync] Unexpected error: " + ex);
                return new RecipeList();
            }
        }

        #region Database actions

        private async Task SafeCreateTableAsync<T>() where T : new()
        {
            try
            {
                if (db == null)
                    return;

                await db.CreateTableAsync<T>();
            }
            catch (SQLiteException sqliteEx)
            {
                Debug.WriteLine($"[SafeCreateTableAsync] SQLite error creating table {typeof(T).Name}: {sqliteEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SafeCreateTableAsync] Unexpected error creating table {typeof(T).Name}: {ex.Message}");
            }
        }


        private async Task MigrateDatabaseAsync(int fromVersion, int toVersion)
        {
            if (db == null) return;

            try
            {
                if (fromVersion < 2 && toVersion >= 2)
                {
                    // Example: adding a "Description" column to ModelItem
                    //await db.ExecuteAsync("ALTER TABLE ModelItem ADD COLUMN Description TEXT");
                }

                // Add more if-conditions for future migrations
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MigrateDatabaseAsync] Migration failed: {ex.Message}");
            }
        }
        #endregion
    }
}
