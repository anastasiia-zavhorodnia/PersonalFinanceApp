using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModels; // <-- ДОДАНО ДЛЯ ДОСТУПУ ДО VIEWMODEL

namespace PersonalFinanceApp
{
    public partial class App : Application
    {
        private readonly MainViewModel _mainViewModel; // Зберігаємо посилання на Singleton ViewModel

        // ОНОВЛЕНО: Конструктор тепер приймає MainViewModel з DI-контейнера
        public App(MainViewModel mainViewModel)
        {
            InitializeComponent();
            _mainViewModel = mainViewModel;
            Debug.WriteLine("[App] Конструктор");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Debug.WriteLine("[App] CreateWindow");

            var window = new Window(new AppShell());

            window.Created += (s, e) => Debug.WriteLine("[Window] Created");
            window.Activated += (s, e) => Debug.WriteLine("[Window] Activated");
            window.Deactivated += (s, e) => Debug.WriteLine("[Window] Deactivated");
            window.Stopped += (s, e) => Debug.WriteLine("[Window] Stopped");
            window.Resumed += (s, e) => Debug.WriteLine("[Window] Resumed");
            window.Destroying += (s, e) => Debug.WriteLine("[Window] Destroying");

            return window;
        }

        protected override void OnStart()
        {
            Debug.WriteLine("[App] OnStart");
            base.OnStart();
        }

        // ОНОВЛЕНО: При переході в режим сну (згортанні) зберігаємо стан чернетки
        protected override void OnSleep()
        {
            Debug.WriteLine("[App] OnSleep");
            _mainViewModel.SaveState(); // Викликаємо збереження форми
            base.OnSleep();
        }

        protected override void OnResume()
        {
            Debug.WriteLine("[App] OnResume");
            base.OnResume();
        }
    }
}
