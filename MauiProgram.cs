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

            
            builder.Services.AddSingleton<AccountService>();

           
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            
            builder.Services.AddTransient<AccountDetailViewModel>();
            builder.Services.AddTransient<AccountDetailPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

