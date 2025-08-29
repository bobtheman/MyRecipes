namespace MyRecipes.Components.Pages
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Forms;
    using MyRecipes.Models;
    using MyRecipes.Services.Interface;

    public partial class AddRecipe
    {
        [Inject] IAppStateService AppStateService { get; set; }
        [Inject] IAlertService AlertService { get; set; }
        [Inject] ICameraService CameraService { get; set; }
        [Inject] ILocalizationService LocalizationService { get; set; }
        [Inject] IOfflineDataService OfflineDataService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        private RecipeItem recipeItem = new RecipeItem();

        private EditContext _editContext;

        protected override async Task OnInitializedAsync()
        {
            AppStateService.ShowSpinner = true;
            AppStateService.SelectedPage = LocalizationService["AddRecipe"];

            _editContext = new EditContext(recipeItem);

            AppStateService.ShowSpinner = false;
        }

        private async Task PickImage()
        {
            AppStateService.ShowSpinner = true;

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var base64 = Convert.ToBase64String(ms.ToArray());
                recipeItem.ImagePath = $"data:{result.ContentType};base64,{base64}";
            }

            AppStateService.ShowSpinner = false;
        }

        private async Task TakeImage()
        {
            AppStateService.ShowSpinner = true;

            if (!await CheckAndRequestCameraPermission())
            {
                AppStateService.ShowSpinner = false;
                return;
            }

            var photoBase64 = await CameraService.TakePhotoAsync();

            if (!string.IsNullOrEmpty(photoBase64))
            {
                recipeItem.ImagePath = photoBase64;
            }

            AppStateService.ShowSpinner = false;
        }

        public async Task SaveItem()
        {
            if (!_editContext.Validate())
            {
                return;
            }

            AppStateService.ShowSpinner = true;

            try
            {
                await OfflineDataService.AddItemAsync(new RecipeItem
                {
                    Name = recipeItem.Name
                });

                await AlertService.ShowSuccessAlertAsync(LocalizationService["Success"], LocalizationService["ItemSaved"]);
                //NavigationManager.NavigateTo("/");
            }
            catch (Exception)
            {
                await AlertService.ShowErrorAlertAsync(LocalizationService["Error"], LocalizationService["FailedToSave"]);
            }
            finally
            {
                AppStateService.ShowSpinner = false;
            }
        }

        #region Camera Permission
        private async Task<bool> CheckAndRequestCameraPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await AlertService.ShowErrorAlertAsync(
                            LocalizationService["PermissionRequired"],
                            LocalizationService["CameraPermissionRequired"]);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await AlertService.ShowErrorAlertAsync(
                    LocalizationService["Error"],
                    $"{LocalizationService["CameraPermissionError"]}: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}