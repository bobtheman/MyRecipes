using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;

namespace MyRecipes.Components.Pages.Xaml;

public partial class DeviceCamera : Popup
{
    private readonly IAudioManager _audioManager;

    public DeviceCamera(IAudioManager audioManager)
    {
        InitializeComponent();
        _audioManager = audioManager;
    }

    private async void scanner_Barcode_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        scanner_Barcode.IsDetecting = false;

        try
        {
            string audioFileName = "beep.mp3";
            try
            {
                var player = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(audioFileName));
                player.Play();

                await Task.Delay(500);
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"Audio file '{audioFileName}' not found. Ensure it exists in Resources/Raw with build action set to MauiAsset.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Audio playback error: {ex.Message}");
        }
        finally
        {
            Close(e.Results[0].Value);
        }
    }
}
