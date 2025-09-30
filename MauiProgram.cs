using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using SQLitePCL;
using AICQD.Services;
using AICQD.Config;
using AICQD.Data;

namespace AICQD
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Batteries_V2.Init();

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // ===== HTTP CLIENTS =====
            // API Service Client
            builder.Services.AddHttpClient<IAicqdApiService, AicqdApiService>(client =>
            {
                client.BaseAddress = new Uri(AppSettings.ApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Gemini AI Service Client
            builder.Services.AddHttpClient<IGeminiAIService, GeminiAIService>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            // ===== BLE SERVICES (Plugin.BLE) =====
            builder.Services.AddSingleton<IBluetoothLE>(CrossBluetoothLE.Current);
            builder.Services.AddSingleton<IAdapter>(CrossBluetoothLE.Current.Adapter);
            builder.Services.AddSingleton<BleClient>();

            // ===== LOCAL DATABASE & REPOSITORIES =====
            builder.Services.AddSingleton<AppDatabase>();
            builder.Services.AddSingleton<DeviceRepository>();
            builder.Services.AddSingleton<DeviceManager>();

            // ===== PAGES =====
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

            var app = builder.Build();

            // Initialize database on startup
            var database = app.Services.GetRequiredService<AppDatabase>();
            _ = database.InitializeAsync();

            return app;
        }
    }
}