using Microsoft.AspNetCore.Components.Web;

namespace MyRecipes.Components.Layout
{
    public partial class MainLayout
    {
        private bool menuOpen = false;

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