using PersonalFinanceApp.Helpers;
using PersonalFinanceApp.Views;

namespace PersonalFinanceApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(Routes.AccountDetail, typeof(AccountDetailPage));
    }
}