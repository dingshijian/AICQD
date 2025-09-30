// Views/PlaceholderPage.cs
using Microsoft.Maui.Controls;

namespace AICQD.Views
{
    public class PlaceholderPage : ContentPage
    {
        public PlaceholderPage()
        {
            // The call to InitializeComponent() is removed.
            // We can define the UI in code instead.
            Content = new VerticalStackLayout
            {
                Children = {
                    new Label { Text = "This is a placeholder page.", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
                }
            };
        }
    }
}