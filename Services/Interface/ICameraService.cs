namespace MyRecipes.Services.Interface
{
    public interface ICameraService
    {
        Task<string?> TakePhotoAsync();
    }
}
