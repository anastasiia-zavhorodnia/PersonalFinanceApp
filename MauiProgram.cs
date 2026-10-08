using Microsoft.Extensions.Logging;
using PersonalFinanceApp.ViewModels;
using PersonalFinanceApp.Views;
using PersonalFinanceApp.Services;
using PersonalFinanceApp.Helpers;
using System.Diagnostics;

namespace PersonalFinanceApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Сервіс бази даних SQLite (Singleton)
            builder.Services.AddSingleton<DatabaseService>();

            // Головна ViewModel тепер зареєстрована як Singleton для збереження стану
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            // Другий екран та його ViewModel (Transient)
            builder.Services.AddTransient<AccountDetailViewModel>();
            builder.Services.AddTransient<AccountDetailPage>();

            
            var logPath = Path.Combine(FileSystem.AppDataDirectory, "logs", "app.log");

            builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // прибираємо шум самого фреймворку

#if DEBUG
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

            builder.Logging.AddProvider(new FileLoggerProvider(logPath, LogLevel.Information));

            Debug.WriteLine($"[Log] Файл логу: {logPath}"); // вивід шляху для звіту

            return builder.Build();
        }
    }
}

