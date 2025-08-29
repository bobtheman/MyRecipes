using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MyRecipes.Services.Interface;
using System.Globalization;

namespace MyRecipes.Components.Layout
{
    public partial class MainLayout
    {
        [Inject] IAppStateService AppStateService { get; set; }
        [Inject] IAlertService AlertService { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        private bool IsDarkMode;
 
        private bool menuOpen = false;
        private Action? _appStateChangedHandler;

        protected override async Task OnInitializedAsync()
        {
            AppStateService.ShowSpinner = true;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-GB");

            //await UpdateCheckerService.CheckForUpdateAsync();

            _appStateChangedHandler = () =>
            {
                InvokeAsync(StateHasChanged);
            };

            AppStateService.OnChange += _appStateChangedHandler;
            AlertService.RegisterRefreshCallback(StateHasChanged);

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