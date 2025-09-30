using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

namespace AICQD
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            Debug.WriteLine("[App] Constructor started");
            
            // Use MainPage directly instead of AppShell for testing
            MainPage = new MainPage();
            
            Debug.WriteLine("[App] MainPage set to MainPage");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Debug.WriteLine("[App] CreateWindow called");
            
            var window = base.CreateWindow(activationState);
            
            // Set explicit window properties
            window.Title = "AICQD - Vape Tracking";
            window.Width = 1000;
            window.Height = 700;
            window.X = 100;
            window.Y = 100;
            
            // Add event handlers to track window lifecycle
            window.Created += (s, e) => Debug.WriteLine("[Window] Created");
            window.Activated += (s, e) => Debug.WriteLine("[Window] Activated");
            window.Deactivated += (s, e) => Debug.WriteLine("[Window] Deactivated");
            window.Stopped += (s, e) => Debug.WriteLine("[Window] Stopped");
            window.Destroying += (s, e) => Debug.WriteLine("[Window] Destroying");
            
            Debug.WriteLine($"[Window] Initialized - Size: {window.Width}x{window.Height}");
            
            return window;
        }
    }
}