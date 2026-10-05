using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalFinanceApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
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

        protected override void OnSleep()
        {
            Debug.WriteLine("[App] OnSleep");
            base.OnSleep();
        }

        protected override void OnResume()
        {
            Debug.WriteLine("[App] OnResume");
            base.OnResume();
        }
    }
}