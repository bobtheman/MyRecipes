namespace MyRecipes.Services
{
    using MyRecipes.Services.Interface;
    using System.Diagnostics;

    public class CameraService : ICameraService
    {
        public async Task<string?> TakePhotoAsync()
        {
            try
            {
#if ANDROID
                // Always request Camera permission
                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();

                // Request correct storage/media permission depending on OS version
                PermissionStatus mediaStatus;
                if (DeviceInfo.Platform == DevicePlatform.Android && DeviceInfo.Version.Major >= 13)
                {
                    // Android 13+ (API 33) → request Media (images) permission
                    mediaStatus = await Permissions.RequestAsync<Permissions.Media>();
                }
                else
                {
                    // Android 12 and below
                    mediaStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                if (cameraStatus != PermissionStatus.Granted || mediaStatus != PermissionStatus.Granted)
                {
                    Debug.WriteLine("Camera or storage/media permission not granted.");
                    return null;
                }
#endif

                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    Debug.WriteLine("No camera available or no app supports image capture.");
                    return null;
                }

                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                    return null;

                using var stream = await photo.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);

                var imageBytes = ms.ToArray();
                return $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CameraService exception: {ex.Message}");
                return null;
            }
        }
    }
}
