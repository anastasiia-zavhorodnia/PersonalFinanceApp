using Microsoft.Extensions.Logging;
using PersonalFinanceApp.ViewModels;
using PersonalFinanceApp.Views;
using PersonalFinanceApp.Services;

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

            // Спільний сервіс даних (Singleton)
            builder.Services.AddSingleton<AccountService>();

            // ОНОВЛЕНО: Головна ViewModel тепер зареєстрована як Singleton для збереження стану
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            // Другий екран та його ViewModel (Transient)
            builder.Services.AddTransient<AccountDetailViewModel>();
            builder.Services.AddTransient<AccountDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

