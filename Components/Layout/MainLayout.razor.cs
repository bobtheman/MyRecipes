using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace MyRecipes.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] IJSRuntime JSRuntime { get; set; }

        private bool IsDarkMode;
 
        private bool menuOpen = false;

        protected override void OnInitialized()
        {
            OnLoad();
            base.OnInitialized();
        }

        private async void OnLoad()
        {
            var storedDarkMode = await SecureStorage.GetAsync("isDarkMode");

            if (storedDarkMode == null)
            {
                var isDeviceDark = Application.Current?.RequestedTheme == AppTheme.Dark;
                await SecureStorage.SetAsync("isDarkMode", isDeviceDark.ToString().ToLower());
                IsDarkMode = isDeviceDark;
            }
            else
            {
                IsDarkMode = storedDarkMode == "true";
            }
            await SetDarkModeClass(IsDarkMode);

            StateHasChanged();
        }

        private async Task SetDarkModeClass(bool enable)
        {
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("setDarkModeClass", enable);
            }
        }

        private void ToggleMenu(MouseEventArgs args)
        {
            menuOpen = !menuOpen;
        }

        private void CloseMenu()
        {
            menuOpen = false;
        }
    }
}