using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace AICQD
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Add debug output to verify Shell is created
            System.Diagnostics.Debug.WriteLine("[AppShell] Constructor called");
            
            // Navigate to MainPage after Shell is initialized using Dispatcher
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(100); // Small delay to ensure Shell is ready
                await GoToAsync("//Dashboard");
                System.Diagnostics.Debug.WriteLine("[AppShell] Navigation to Dashboard completed");
            });
        }
    }
}